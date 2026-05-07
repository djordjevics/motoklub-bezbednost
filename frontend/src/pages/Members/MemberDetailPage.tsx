import { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import {
  Alert,
  Box,
  Chip,
  CircularProgress,
  IconButton,
  Paper,
  Stack,
  Tab,
  Tabs,
  Typography,
} from '@mui/material'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import { useTranslation } from 'react-i18next'
import { memberService } from '../../services/memberService'
import { memberTypeColorOrFallback } from '../../utils/memberTypeColor'
import ProfileTab from '../../components/MemberDetail/ProfileTab'
import MotorcyclesTab from '../../components/MemberDetail/MotorcyclesTab'
import EquipmentTab from '../../components/MemberDetail/EquipmentTab'
import TrainingsTab from '../../components/MemberDetail/TrainingsTab'
import PaymentsTab from '../../components/MemberDetail/PaymentsTab'
import TagsTab from '../../components/MemberDetail/TagsTab'
import CommentsTab from '../../components/MemberDetail/CommentsTab'

const MemberDetailPage = () => {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const { t } = useTranslation()
  const memberId = Number(id)
  const [tab, setTab] = useState(0)

  const { data: member, isLoading, error } = useQuery({
    queryKey: ['member', memberId],
    queryFn: () => memberService.getById(memberId),
    enabled: !Number.isNaN(memberId),
  })

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 400 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (error || !member) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography color="error">{t('common.error')}</Typography>
      </Box>
    )
  }

  const memberTypeName = member.memberType?.typeName ?? ''
  const memberTypeColor = memberTypeColorOrFallback(member.memberType?.color)
  const recordLockedInactive = Boolean(member.membershipExemptManual)
  const typePaysMembership = member.memberType ? member.memberType.paidMembership !== false : true
  const ageExempt = Boolean(member.isMembershipPaymentExemptDueToAge)

  return (
    <Box>
      <Stack direction="row" alignItems="center" spacing={2} sx={{ mb: 2 }}>
        <IconButton aria-label={t('common.back')} onClick={() => navigate(-1)}>
          <ArrowBackIcon />
        </IconButton>
        <Typography variant="h4" sx={{ flexGrow: 1 }}>
          {member.name} {member.surname}
        </Typography>
        {memberTypeName && (
          <Chip
            label={memberTypeName}
            sx={{ backgroundColor: memberTypeColor, color: '#fff', fontWeight: 600 }}
          />
        )}
      </Stack>

      {member.membershipExemptManual && (
        <Alert severity="warning" sx={{ mb: 2 }}>
          {t('members.exemptInactiveBanner')}
        </Alert>
      )}
      {!recordLockedInactive && typePaysMembership && ageExempt && (
        <Alert severity="info" sx={{ mb: 2 }}>
          {t('members.paymentExemptAgeBanner')}
        </Alert>
      )}

      <Paper sx={{ mb: 2 }}>
        <Tabs
          value={tab}
          onChange={(_, value) => setTab(value)}
          variant="scrollable"
          scrollButtons="auto"
        >
          <Tab label={t('memberDetail.tabs.profile')} />
          <Tab label={t('memberDetail.tabs.motorcycles')} />
          <Tab label={t('memberDetail.tabs.equipment')} />
          <Tab label={t('memberDetail.tabs.trainings')} />
          <Tab label={t('memberDetail.tabs.payments')} />
          <Tab label={t('memberDetail.tabs.tags')} />
          <Tab label={t('memberDetail.tabs.comments')} />
        </Tabs>
      </Paper>

      <Box>
        {tab === 0 && <ProfileTab memberId={memberId} member={member} />}
        {tab === 1 && <MotorcyclesTab memberId={memberId} recordLocked={recordLockedInactive} />}
        {tab === 2 && <EquipmentTab memberId={memberId} recordLocked={recordLockedInactive} />}
        {tab === 3 && <TrainingsTab memberId={memberId} recordLocked={recordLockedInactive} />}
        {tab === 4 && (
          <PaymentsTab
            memberId={memberId}
            isInactiveMemberRecord={Boolean(member.membershipExemptManual)}
            isMembershipPaymentRequired={Boolean(member.isMembershipPaymentRequired)}
            isMembershipPaymentExemptDueToAge={ageExempt}
            memberTypePaysMembership={typePaysMembership}
          />
        )}
        {tab === 5 && <TagsTab memberId={memberId} recordLocked={recordLockedInactive} />}
        {tab === 6 && <CommentsTab memberId={memberId} recordLocked={recordLockedInactive} />}
      </Box>
    </Box>
  )
}

export default MemberDetailPage
