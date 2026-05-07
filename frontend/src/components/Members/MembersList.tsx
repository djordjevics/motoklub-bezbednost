import { useEffect, useMemo, useState, type SyntheticEvent } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { keepPreviousData, useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Box,
  Button,
  Chip,
  CircularProgress,
  FormControlLabel,
  IconButton,
  InputAdornment,
  MenuItem,
  Paper,
  Select,
  Stack,
  Switch,
  ToggleButton,
  ToggleButtonGroup,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from '@mui/material'
import type { SelectChangeEvent } from '@mui/material/Select'
import AddIcon from '@mui/icons-material/Add'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'
import SearchIcon from '@mui/icons-material/Search'
import { memberService } from '../../services/memberService'
import { lookupsService } from '../../services/lookupsService'
import { memberTypeColorOrFallback } from '../../utils/memberTypeColor'
import { ConfirmDialog } from '../../components/common/ConfirmDialog'
import type { Member } from '../../types/Member'
import type { MemberType } from '../../types/MemberType'
import { formatIsoDateDdMmYyyyOrDash } from '../../utils/dateFormat'
import { formatActiveMemberTagLabel } from '../../utils/tagActive'

interface MembersListProps {
  onEdit: (memberId: number) => void
  onAddNew: () => void
  onDelete?: (member: Member) => void
}

type MemberTypeFilter = number | 'all'

type TrainingFilter = 'all' | 'with' | 'without'

function readTrainingFilter(searchParams: URLSearchParams): TrainingFilter {
  const v = searchParams.get('training')
  if (v === 'with' || v === 'without') return v
  return 'all'
}

function readIncludeInactive(searchParams: URLSearchParams): boolean {
  return searchParams.get('inactive') === 'all'
}

const MembersList = ({ onEdit, onAddNew, onDelete }: MembersListProps) => {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const [searchParams, setSearchParams] = useSearchParams()
  const queryClient = useQueryClient()

  const trainingFilter = readTrainingFilter(searchParams)
  const includeInactive = readIncludeInactive(searchParams)

  const [searchInput, setSearchInput] = useState('')
  const [searchQuery, setSearchQuery] = useState('')
  const [selectedMemberTypeId, setSelectedMemberTypeId] = useState<MemberTypeFilter>('all')
  const [memberToDelete, setMemberToDelete] = useState<Member | null>(null)

  useEffect(() => {
    const handle = setTimeout(() => setSearchQuery(searchInput.trim()), 300)
    return () => clearTimeout(handle)
  }, [searchInput])

  const { data: memberTypes } = useQuery<MemberType[]>({
    queryKey: ['memberTypes'],
    queryFn: lookupsService.getMemberTypes,
  })

  const {
    data: members,
    isLoading,
    isError,
  } = useQuery<Member[]>({
    queryKey: ['members', searchQuery],
    queryFn: () => (searchQuery ? memberService.search(searchQuery) : memberService.getAll()),
    placeholderData: keepPreviousData,
  })

  const deleteMutation = useMutation({
    mutationFn: (id: number) => memberService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['members'] })
    },
  })

  const filtered = useMemo(() => {
    if (!members) return []
    let list = members
    if (selectedMemberTypeId !== 'all') {
      list = list.filter((m) => m.memberTypeId === selectedMemberTypeId)
    }
    if (trainingFilter === 'with') {
      list = list.filter((m) => (m.trainings?.length ?? 0) > 0)
    } else if (trainingFilter === 'without') {
      list = list.filter((m) => (m.trainings?.length ?? 0) === 0)
    }
    if (!includeInactive) {
      list = list.filter((m) => !m.membershipExemptManual)
    }
    return list
  }, [members, selectedMemberTypeId, trainingFilter, includeInactive])

  const handleTrainingFilterChange = (_: SyntheticEvent, value: TrainingFilter | null) => {
    if (value == null) return
    setSearchParams(
      (prev) => {
        const p = new URLSearchParams(prev)
        if (value === 'all') p.delete('training')
        else p.set('training', value)
        return p
      },
      { replace: true },
    )
  }

  const handleTypeChange = (event: SelectChangeEvent<string>) => {
    const value = event.target.value
    setSelectedMemberTypeId(value === 'all' ? 'all' : Number(value))
  }

  const handleIncludeInactiveChange = (_: SyntheticEvent, checked: boolean) => {
    setSearchParams(
      (prev) => {
        const p = new URLSearchParams(prev)
        if (checked) p.set('inactive', 'all')
        else p.delete('inactive')
        return p
      },
      { replace: true },
    )
  }

  const handleRowClick = (memberId: number) => {
    navigate(`/members/${memberId}`)
  }

  const requestDelete = (member: Member) => {
    setMemberToDelete(member)
  }

  const confirmDelete = () => {
    if (!memberToDelete) return
    const member = memberToDelete
    setMemberToDelete(null)
    deleteMutation.mutate(member.id, {
      onSuccess: () => {
        onDelete?.(member)
      },
    })
  }

  const cancelDelete = () => {
    setMemberToDelete(null)
  }

  return (
    <Box>
      <Stack
        direction={{ xs: 'column', md: 'row' }}
        spacing={2}
        alignItems={{ xs: 'stretch', md: 'center' }}
        sx={{ mb: 2 }}
      >
        <TextField
          size="small"
          placeholder={t('members.search.placeholder')}
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon fontSize="small" />
              </InputAdornment>
            ),
          }}
          sx={{ flex: { md: 1 }, minWidth: { md: 240 } }}
        />
        <Select
          size="small"
          value={selectedMemberTypeId === 'all' ? 'all' : String(selectedMemberTypeId)}
          onChange={handleTypeChange}
          displayEmpty
          sx={{ minWidth: 200 }}
          aria-label={t('members.filters.type')}
        >
          <MenuItem value="all">{t('members.filters.allTypes')}</MenuItem>
          {memberTypes?.map((mt) => (
            <MenuItem key={mt.id} value={String(mt.id)}>
              {mt.typeName ?? `#${mt.id}`}
            </MenuItem>
          ))}
        </Select>
        <ToggleButtonGroup
          size="small"
          exclusive
          value={trainingFilter}
          onChange={handleTrainingFilterChange}
          aria-label={t('members.filters.training')}
          sx={{ flexWrap: 'wrap' }}
        >
          <ToggleButton value="all">{t('members.filters.trainingAll')}</ToggleButton>
          <ToggleButton value="with">{t('members.filters.trainingWith')}</ToggleButton>
          <ToggleButton value="without">{t('members.filters.trainingWithout')}</ToggleButton>
        </ToggleButtonGroup>
        <FormControlLabel
          sx={{ m: 0 }}
          control={
            <Switch
              size="small"
              checked={includeInactive}
              onChange={handleIncludeInactiveChange}
              inputProps={{ 'aria-label': t('members.filters.includeInactive') }}
            />
          }
          label={t('members.filters.includeInactive')}
        />
        <Box sx={{ flex: { md: '0 0 auto' }, ml: { md: 'auto' } }}>
          <Button variant="contained" startIcon={<AddIcon />} onClick={onAddNew}>
            {t('members.addNew')}
          </Button>
        </Box>
      </Stack>

      <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
        {t('members.count', { count: filtered.length })}
      </Typography>

      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 200 }}>
          <CircularProgress />
        </Box>
      ) : isError ? (
        <Box sx={{ p: 2 }}>
          <Typography color="error">{t('common.error')}</Typography>
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>{t('members.fields.name')}</TableCell>
                <TableCell>{t('members.fields.surname')}</TableCell>
                <TableCell>{t('members.fields.memberType')}</TableCell>
                <TableCell>{t('members.activeTagColumn')}</TableCell>
                <TableCell>{t('members.fields.phone')}</TableCell>
                <TableCell>{t('members.fields.email')}</TableCell>
                <TableCell>{t('members.fields.registeredOn')}</TableCell>
                <TableCell align="right">{t('common.actions')}</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {filtered.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={8} align="center">
                    <Typography variant="body2" color="text.secondary" sx={{ py: 2 }}>
                      {t('common.noData')}
                    </Typography>
                  </TableCell>
                </TableRow>
              ) : (
                filtered.map((member) => {
                  const inactiveLocked = Boolean(member.membershipExemptManual)
                  const color = memberTypeColorOrFallback(member.memberType?.color)
                  const activeTagLabel = formatActiveMemberTagLabel(member)
                  return (
                    <TableRow
                      key={member.id}
                      hover
                      onClick={() => handleRowClick(member.id)}
                      sx={{
                        borderLeft: `4px solid ${color}`,
                        cursor: 'pointer',
                      }}
                    >
                      <TableCell>{member.name}</TableCell>
                      <TableCell>{member.surname}</TableCell>
                      <TableCell>
                        <Stack direction="row" spacing={1} alignItems="center" flexWrap="wrap">
                          <Chip
                            size="small"
                            label={member.memberType?.typeName ?? '-'}
                            sx={{ backgroundColor: color, color: '#fff' }}
                          />
                          {member.membershipExemptManual && (
                            <Chip
                              size="small"
                              label={t('members.chipInactiveRecord')}
                              color="warning"
                              variant="outlined"
                            />
                          )}
                        </Stack>
                      </TableCell>
                      <TableCell>
                        {activeTagLabel ? (
                          <Chip size="small" label={activeTagLabel} variant="outlined" color="primary" />
                        ) : (
                          <Typography variant="body2" color="text.secondary">
                            —
                          </Typography>
                        )}
                      </TableCell>
                      <TableCell>{member.mobilePhone || '-'}</TableCell>
                      <TableCell>{member.email || '-'}</TableCell>
                      <TableCell>{formatIsoDateDdMmYyyyOrDash(member.registeredOn)}</TableCell>
                      <TableCell
                        align="right"
                        onClick={(e) => e.stopPropagation()}
                        sx={{ whiteSpace: 'nowrap' }}
                      >
                        <IconButton
                          size="small"
                          aria-label={t('common.edit')}
                          onClick={(e) => {
                            e.stopPropagation()
                            if (!inactiveLocked) onEdit(member.id)
                          }}
                          disabled={inactiveLocked}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                        <IconButton
                          size="small"
                          aria-label={t('common.delete')}
                          color="error"
                          onClick={(e) => {
                            e.stopPropagation()
                            if (!inactiveLocked) requestDelete(member)
                          }}
                          disabled={inactiveLocked}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  )
                })
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <ConfirmDialog
        open={memberToDelete !== null}
        title={t('members.delete.title')}
        message={
          memberToDelete
            ? t('members.delete.message', {
                name: `${memberToDelete.name} ${memberToDelete.surname}`.trim(),
              })
            : undefined
        }
        confirmText={t('members.delete.confirm')}
        destructive
        onConfirm={confirmDelete}
        onCancel={cancelDelete}
      />
    </Box>
  )
}

export default MembersList
