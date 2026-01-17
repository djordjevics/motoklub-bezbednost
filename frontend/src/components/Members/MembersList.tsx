import { useQuery } from '@tanstack/react-query'
import { Box, Button, CircularProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material'
import { memberService } from '../../services/memberService'
import { Member } from '../../types/Member'
import AddIcon from '@mui/icons-material/Add'

interface MembersListProps {
  onSelectMember: (memberId: number) => void
  onAddNew: () => void
}

const MembersList = ({ onSelectMember, onAddNew }: MembersListProps) => {
  const { data: members, isLoading, error } = useQuery<Member[]>({
    queryKey: ['members'],
    queryFn: memberService.getAll,
  })

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 200 }}>
        <CircularProgress />
      </Box>
    )
  }

  if (error) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography color="error">Error loading members. Please try again later.</Typography>
      </Box>
    )
  }

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <Typography variant="body1" color="text.secondary">
          {members?.length || 0} member(s) found
        </Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={onAddNew}>
          Add New Member
        </Button>
      </Box>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Surname</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Phone</TableCell>
              <TableCell>Registered On</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {members && members.length > 0 ? (
              members.map((member) => (
                <TableRow
                  key={member.id}
                  hover
                  sx={{ cursor: 'pointer' }}
                  onClick={() => onSelectMember(member.id)}
                >
                  <TableCell>{member.id}</TableCell>
                  <TableCell>{member.name}</TableCell>
                  <TableCell>{member.surname}</TableCell>
                  <TableCell>{member.email || '-'}</TableCell>
                  <TableCell>{member.mobilePhone || '-'}</TableCell>
                  <TableCell>{member.registeredOn ? new Date(member.registeredOn).toLocaleDateString() : '-'}</TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={6} align="center">
                  <Typography variant="body2" color="text.secondary" sx={{ py: 2 }}>
                    No members found
                  </Typography>
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}

export default MembersList

