import { DatePicker } from '@mui/x-date-pickers/DatePicker'
import dayjs from 'dayjs'

export interface FormDatePickerProps {
  label: string
  value?: string
  onChange: (isoDateYyyyMmDd: string) => void
  error?: boolean
  helperText?: string
  disabled?: boolean
  fullWidth?: boolean
  size?: 'small' | 'medium'
  required?: boolean
}

const FormDatePicker = ({
  label,
  value,
  onChange,
  error,
  helperText,
  disabled,
  fullWidth,
  size = 'small',
  required,
}: FormDatePickerProps) => {
  const iso = value ?? ''
  const parsed = iso && dayjs(iso).isValid() ? dayjs(iso) : null
  return (
    <DatePicker
      label={label}
      format="DD/MM/YYYY"
      value={parsed}
      onChange={(v) => {
        if (!v) onChange('')
        else onChange(v.format('YYYY-MM-DD'))
      }}
      disabled={disabled}
      slotProps={{
        textField: {
          required,
          error,
          helperText,
          fullWidth,
          size,
        },
      }}
    />
  )
}

export default FormDatePicker
