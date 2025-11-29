import { Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material'
import { useNavigate, useLocation } from 'react-router-dom'
import DashboardIcon from '@mui/icons-material/Dashboard'
import PeopleIcon from '@mui/icons-material/People'
import TwoWheelerIcon from '@mui/icons-material/TwoWheeler'
import SchoolIcon from '@mui/icons-material/School'
import InventoryIcon from '@mui/icons-material/Inventory'

const drawerWidth = 240

const menuItems = [
  { text: 'Dashboard', icon: <DashboardIcon />, path: '/dashboard' },
  { text: 'Members', icon: <PeopleIcon />, path: '/members' },
  { text: 'Motorcycles', icon: <TwoWheelerIcon />, path: '/motorcycles' },
  { text: 'Trainings', icon: <SchoolIcon />, path: '/trainings' },
  { text: 'Equipment', icon: <InventoryIcon />, path: '/equipment' },
]

const Sidebar = () => {
  const navigate = useNavigate()
  const location = useLocation()

  return (
    <Drawer
      variant="permanent"
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
          marginTop: '64px',
        },
      }}
    >
      <List>
        {menuItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton
              selected={location.pathname === item.path}
              onClick={() => navigate(item.path)}
            >
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Drawer>
  )
}

export default Sidebar

