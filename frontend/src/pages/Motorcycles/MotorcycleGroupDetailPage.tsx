import type { ReactNode } from 'react'
import {
  Box,
  Button,
  CircularProgress,
  Divider,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from '@mui/material'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import { useNavigate, useParams } from 'react-router-dom'
import { useMemo } from 'react'
import { useQuery } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { memberService } from '../../services/memberService'
import {
  buildMotorcycleModelGroups,
  decodeMotorcycleGroupSegment,
  findMotorcycleModelGroup,
} from '../../utils/motorcycleGrouping'
import { formatMotorcycleBrandModelPlate } from '../../utils/motorcycleLabel'

const MotorcycleGroupDetailPage = () => {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const { brandKey = '', modelKey = '' } = useParams<{ brandKey: string; modelKey: string }>()

  const brandDecoded = decodeMotorcycleGroupSegment(brandKey)
  const modelDecoded = decodeMotorcycleGroupSegment(modelKey)

  const { data: members, isLoading, error } = useQuery({
    queryKey: ['members'],
    queryFn: memberService.getAll,
  })

  const group = useMemo(() => {
    const groups = buildMotorcycleModelGroups(members ?? [])
    return findMotorcycleModelGroup(groups, brandDecoded, modelDecoded)
  }, [members, brandDecoded, modelDecoded])

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 6 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (error) {
    return (
      <Typography color="error" sx={{ p: 2 }}>
        {t('common.error')}
      </Typography>
    )
  }

  if (!group || group.items.length === 0) {
    return (
      <Box sx={{ p: 2 }}>
        <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/motorcycles')}>
          {t('common.back')}
        </Button>
        <Typography sx={{ mt: 2 }} color="text.secondary">
          {t('common.noData')}
        </Typography>
      </Box>
    )
  }

  const rep = group.items[0]?.motorcycle

  return (
    <Box>
      <Stack direction="row" alignItems="center" spacing={1} sx={{ mb: 2 }}>
        <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/motorcycles')}>
          {t('common.back')}
        </Button>
      </Stack>
      <Typography variant="h4" gutterBottom>
        {group.brandName} · {group.modelName}
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        {t('motorcyclesOverview.membersWithModel')}: {group.memberCount}
      </Typography>

      <Paper sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          {t('motorcyclesOverview.specHeading')}
        </Typography>
        {rep ? (
          <Stack spacing={1} divider={<Divider flexItem />} sx={{ maxWidth: 480 }}>
            <Row label={t('motorcycles.fields.commercialName')} value={rep.commercialName ?? '—'} />
            <Row label={t('motorcycles.fields.engineDisplacment')} value={rep.engineDisplacment ?? '—'} />
            <Row label={t('motorcycles.fields.enginePower')} value={rep.enginePower ?? '—'} />
            <Row label={t('motorcycles.fields.color')} value={rep.color ?? '—'} />
          </Stack>
        ) : (
          <Typography variant="body2">{t('common.noData')}</Typography>
        )}
        <Typography variant="caption" color="text.secondary" display="block" sx={{ mt: 2 }}>
          {t('motorcyclesOverview.specNote')}
        </Typography>
      </Paper>

      <Typography variant="h6" sx={{ mb: 1 }}>
        {t('motorcyclesOverview.ownersHeading')}
      </Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('members.fields.name')}</TableCell>
              <TableCell>{t('members.fields.surname')}</TableCell>
              <TableCell>{t('motorcycles.fields.registerPlate')}</TableCell>
              <TableCell>{t('trainings.fields.motorcycle')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {group.items.map(({ motorcycle, member }) => (
              <TableRow
                key={`${motorcycle.id}-${member.id}`}
                hover
                sx={{ cursor: 'pointer' }}
                onClick={() => navigate(`/members/${member.id}`)}
              >
                <TableCell>{member.name}</TableCell>
                <TableCell>{member.surname}</TableCell>
                <TableCell>{motorcycle.registerPlate ?? '—'}</TableCell>
                <TableCell>{formatMotorcycleBrandModelPlate(motorcycle)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}

function Row({ label, value }: { label: string; value: ReactNode }) {
  return (
    <Stack direction={{ xs: 'column', sm: 'row' }} spacing={{ xs: 0, sm: 2 }}>
      <Typography component="span" variant="caption" color="text.secondary" sx={{ minWidth: 160 }}>
        {label}
      </Typography>
      <Typography component="span" variant="body2">
        {value}
      </Typography>
    </Stack>
  )
}

export default MotorcycleGroupDetailPage
