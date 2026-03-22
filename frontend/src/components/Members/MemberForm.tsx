type MemberFormProps = {
  memberId: number | null
  onClose: () => void
}

const MemberForm = ({ memberId, onClose }: MemberFormProps) => {
  return (
    <div>
      Member Form Component (memberId: {memberId === null ? 'new' : memberId})
      <button type="button" onClick={onClose}>
        Close
      </button>
    </div>
  )
}

export default MemberForm

