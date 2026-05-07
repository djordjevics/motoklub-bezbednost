import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@mui/material'
import { useTranslation } from 'react-i18next'

interface Props {
  open: boolean
  title: string
  message?: string
  confirmText?: string
  cancelText?: string
  destructive?: boolean
  onConfirm: () => void
  onCancel: () => void
}

export function ConfirmDialog({
  open,
  title,
  message,
  confirmText,
  cancelText,
  destructive = false,
  onConfirm,
  onCancel,
}: Props) {
  const { t } = useTranslation()
  return (
    <Dialog open={open} onClose={onCancel} maxWidth="xs" fullWidth>
      <DialogTitle>{title}</DialogTitle>
      {message && (
        <DialogContent>
          <DialogContentText>{message}</DialogContentText>
        </DialogContent>
      )}
      <DialogActions>
        <Button onClick={onCancel}>{cancelText ?? t('common.cancel')}</Button>
        <Button
          onClick={onConfirm}
          color={destructive ? 'error' : 'primary'}
          variant="contained"
          autoFocus
        >
          {confirmText ?? t('common.confirm')}
        </Button>
      </DialogActions>
    </Dialog>
  )
}
