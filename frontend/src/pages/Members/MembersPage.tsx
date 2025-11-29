import { useState } from 'react'
import { Box, Typography } from '@mui/material'
import MembersList from '../../components/Members/MembersList'
import MemberForm from '../../components/Members/MemberForm'

const MembersPage = () => {
  const [selectedMember, setSelectedMember] = useState<number | null>(null)
  const [isFormOpen, setIsFormOpen] = useState(false)

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Members
      </Typography>
      <MembersList
        onSelectMember={setSelectedMember}
        onAddNew={() => {
          setSelectedMember(null)
          setIsFormOpen(true)
        }}
      />
      {isFormOpen && (
        <MemberForm
          memberId={selectedMember}
          onClose={() => setIsFormOpen(false)}
        />
      )}
    </Box>
  )
}

export default MembersPage

