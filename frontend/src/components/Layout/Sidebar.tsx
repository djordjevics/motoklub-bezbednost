import { Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material'
import { useNavigate, useLocation } from 'react-router-dom'
import DashboardIcon from '@mui/icons-material/Dashboard'
import PeopleIcon from '@mui/icons-material/People'
import SchoolIcon from '@mui/icons-material/School'
import TwoWheelerIcon from '@mui/icons-material/TwoWheeler'
import { useTranslation } from 'react-i18next'
import { clubNavBlue } from '../../theme/brand'

const drawerWidth = 240

const Sidebar = () => {
  const navigate = useNavigate()
  const location = useLocation()
  const { t } = useTranslation()

  const menuItems = [
    { key: 'dashboard', icon: <DashboardIcon />, path: '/dashboard', label: t('nav.dashboard') },
    { key: 'members', icon: <PeopleIcon />, path: '/members', label: t('nav.members') },
    { key: 'motorcycles', icon: <TwoWheelerIcon />, path: '/motorcycles', label: t('nav.motorcycles') },
    { key: 'sessions', icon: <SchoolIcon />, path: '/training-sessions', label: t('nav.trainingSessions') },
  ]

  const isSelected = (path: string) => {
    if (path === '/dashboard') {
      return location.pathname === '/' || location.pathname === '/dashboard'
    }
    if (path === '/motorcycles') {
      return location.pathname === '/motorcycles' || location.pathname.startsWith('/motorcycles/')
    }
    return location.pathname === path || location.pathname.startsWith(`${path}/`)
  }

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
          bgcolor: clubNavBlue,
          color: '#fff',
          borderRight: '1px solid rgba(255,255,255,0.12)',
        },
      }}
    >
      <List sx={{ py: 1 }}>
        {menuItems.map((item) => (
          <ListItem key={item.key} disablePadding>
            <ListItemButton
              selected={isSelected(item.path)}
              onClick={() => navigate(item.path)}
              sx={{
                color: '#fff',
                '&.Mui-selected': {
                  bgcolor: 'rgba(255,255,255,0.14)',
                  '&:hover': { bgcolor: 'rgba(255,255,255,0.18)' },
                },
                '&:hover': { bgcolor: 'rgba(255,255,255,0.08)' },
              }}
            >
              <ListItemIcon sx={{ color: 'inherit', minWidth: 40 }}>{item.icon}</ListItemIcon>
              <ListItemText
                primary={item.label}
                primaryTypographyProps={{ sx: { color: 'inherit', fontWeight: 500 } }}
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Drawer>
  )
}

export default Sidebar
