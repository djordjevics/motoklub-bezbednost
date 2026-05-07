import { useState } from 'react'
import { Box, Typography } from '@mui/material'
import { useTranslation } from 'react-i18next'
import MembersList from '../../components/Members/MembersList'
import MemberForm from '../../components/Members/MemberForm'

const MembersPage = () => {
  const { t } = useTranslation()
  const [editingMemberId, setEditingMemberId] = useState<number | null>(null)
  const [formOpen, setFormOpen] = useState(false)

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        {t('members.title')}
      </Typography>
      <MembersList
        onEdit={(id) => {
          setEditingMemberId(id)
          setFormOpen(true)
        }}
        onAddNew={() => {
          setEditingMemberId(null)
          setFormOpen(true)
        }}
      />
      <MemberForm
        open={formOpen}
        memberId={editingMemberId}
        onClose={() => setFormOpen(false)}
      />
    </Box>
  )
}

export default MembersPage
