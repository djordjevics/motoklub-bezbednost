import { Box, CircularProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { useMemo } from 'react'
import { useTranslation } from 'react-i18next'
import { memberService } from '../../services/memberService'
import { buildMotorcycleModelGroups, encodeMotorcycleGroupSegment } from '../../utils/motorcycleGrouping'

const MotorcyclesOverviewPage = () => {
  const { t } = useTranslation()
  const navigate = useNavigate()

  const { data: members, isLoading, error } = useQuery({
    queryKey: ['members'],
    queryFn: memberService.getAll,
  })

  const groups = useMemo(() => buildMotorcycleModelGroups(members ?? []), [members])

  const handleRow = (brandName: string, modelName: string) => {
    const b = encodeMotorcycleGroupSegment(brandName === '—' ? '' : brandName)
    const m = encodeMotorcycleGroupSegment(modelName === '—' ? '' : modelName)
    navigate(`/motorcycles/${b}/${m}`)
  }

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

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ mb: 2 }}>
        {t('motorcyclesOverview.title')}
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        {t('motorcyclesOverview.subtitle')}
      </Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('motorcycles.fields.brandName')}</TableCell>
              <TableCell>{t('motorcycles.fields.modelName')}</TableCell>
              <TableCell align="right">{t('motorcyclesOverview.membersWithModel')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {groups.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3} align="center">
                  <Typography variant="body2" color="text.secondary" sx={{ py: 3 }}>
                    {t('common.noData')}
                  </Typography>
                </TableCell>
              </TableRow>
            ) : (
              groups.map((g) => (
                <TableRow
                  key={g.normKey}
                  hover
                  sx={{ cursor: 'pointer' }}
                  onClick={() => handleRow(g.brandName, g.modelName)}
                >
                  <TableCell>{g.brandName}</TableCell>
                  <TableCell>{g.modelName}</TableCell>
                  <TableCell align="right">{g.memberCount}</TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}

export default MotorcyclesOverviewPage
