import { useEffect, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Paper,
  Stack,
  TextField,
  Typography,
} from '@mui/material'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'
import { useTranslation } from 'react-i18next'
import { commentService } from '../../services/commentService'
import type { Comment } from '../../types/Comment'
import { ConfirmDialog } from '../common/ConfirmDialog'
import { formatDateTimeDdMmYyyyHm } from '../../utils/dateFormat'

interface CommentsTabProps {
  memberId: number
  recordLocked?: boolean
}

const formatTime = (value?: string): string => formatDateTimeDdMmYyyyHm(value)

const sortByCreationDesc = (a: Comment, b: Comment): number => {
  const ta = a.creationTime ? new Date(a.creationTime).getTime() : 0
  const tb = b.creationTime ? new Date(b.creationTime).getTime() : 0
  return tb - ta
}

const CommentsTab = ({ memberId, recordLocked = false }: CommentsTabProps) => {
  const { t } = useTranslation()
  const queryClient = useQueryClient()
  const queryKey = ['comments', 'byMember', memberId] as const

  const { data, isLoading, error } = useQuery({
    queryKey,
    queryFn: () => commentService.getByMember(memberId),
  })

  const [newText, setNewText] = useState('')
  const [editing, setEditing] = useState<Comment | null>(null)
  const [editText, setEditText] = useState('')
  const [confirmDelete, setConfirmDelete] = useState<Comment | null>(null)

  useEffect(() => {
    setEditText(editing?.commentText ?? '')
  }, [editing])

  const createMutation = useMutation({
    mutationFn: (commentText: string) => commentService.create({ memberId, commentText }),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setNewText('')
    },
  })

  const updateMutation = useMutation({
    mutationFn: async ({ id, commentText }: { id: number; commentText: string }) => {
      await commentService.update(id, { commentText })
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setEditing(null)
    },
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => commentService.delete(id),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey })
      setConfirmDelete(null)
    },
  })

  const handleAdd = () => {
    const text = newText.trim()
    if (text === '') return
    createMutation.mutate(text)
  }

  const handleEditSave = () => {
    if (!editing) return
    const text = editText.trim()
    if (text === '') return
    updateMutation.mutate({ id: editing.id, commentText: text })
  }

  const sorted = [...(data ?? [])].sort(sortByCreationDesc)

  return (
    <Box>
      <Typography variant="h6" sx={{ mb: 2 }}>
        {t('comments.title')}
      </Typography>

      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Typography color="error">{t('common.error')}</Typography>
      ) : sorted.length === 0 ? (
        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
          {t('common.noData')}
        </Typography>
      ) : (
        <Stack spacing={2} sx={{ mb: 3 }}>
          {sorted.map((comment) => {
            const edited =
              !!comment.editTime && !!comment.creationTime && comment.editTime !== comment.creationTime
            return (
              <Paper key={comment.id} sx={{ p: 2 }}>
                <Stack direction="row" justifyContent="space-between" alignItems="flex-start" spacing={1}>
                  <Box sx={{ flexGrow: 1, minWidth: 0 }}>
                    <Typography sx={{ whiteSpace: 'pre-wrap', wordBreak: 'break-word' }}>
                      {comment.commentText}
                    </Typography>
                    <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 1 }}>
                      {formatTime(comment.creationTime)}
                      {edited ? ` • ${t('comments.fields.editedAt')}: ${formatTime(comment.editTime)}` : ''}
                    </Typography>
                  </Box>
                  <Stack direction="row" spacing={0.5}>
                    <IconButton
                      size="small"
                      onClick={() => setEditing(comment)}
                      aria-label={t('common.edit')}
                    >
                      <EditIcon fontSize="small" />
                    </IconButton>
                    <IconButton
                      size="small"
                      color="error"
                      onClick={() => setConfirmDelete(comment)}
                      aria-label={t('common.delete')}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </Stack>
                </Stack>
              </Paper>
            )
          })}
        </Stack>
      )}

      <Paper sx={{ p: 2 }}>
        <TextField
          fullWidth
          multiline
          minRows={3}
          placeholder={t('comments.placeholder')}
          value={newText}
          onChange={(e) => setNewText(e.target.value)}
          disabled={recordLocked}
        />
        <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2 }}>
          <Button
            variant="contained"
            onClick={handleAdd}
            disabled={
              recordLocked || createMutation.isPending || newText.trim() === ''
            }
          >
            {t('comments.addNew')}
          </Button>
        </Box>
      </Paper>

      <Dialog open={!!editing} onClose={() => setEditing(null)} maxWidth="sm" fullWidth>
        <DialogTitle>{t('common.edit')}</DialogTitle>
        <DialogContent dividers>
          <TextField
            fullWidth
            multiline
            minRows={4}
            value={editText}
            onChange={(e) => setEditText(e.target.value)}
            placeholder={t('comments.placeholder')}
            autoFocus
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setEditing(null)}>{t('common.cancel')}</Button>
          <Button
            variant="contained"
            onClick={handleEditSave}
            disabled={updateMutation.isPending || editText.trim() === ''}
          >
            {t('common.save')}
          </Button>
        </DialogActions>
      </Dialog>

      <ConfirmDialog
        open={!!confirmDelete}
        title={t('common.delete')}
        destructive
        confirmText={t('common.delete')}
        onConfirm={() => confirmDelete && deleteMutation.mutate(confirmDelete.id)}
        onCancel={() => setConfirmDelete(null)}
      />
    </Box>
  )
}

export default CommentsTab
