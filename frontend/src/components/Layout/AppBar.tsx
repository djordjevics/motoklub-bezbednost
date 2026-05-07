import { useState } from 'react'
import { AppBar as MuiAppBar, Box, IconButton, Menu, MenuItem, Toolbar, Typography } from '@mui/material'
import LanguageIcon from '@mui/icons-material/Language'
import CheckIcon from '@mui/icons-material/Check'
import { useTranslation } from 'react-i18next'
import { clubLogoSrc, clubNavBlue } from '../../theme/brand'

const AppBar = () => {
  const { t, i18n } = useTranslation()
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const open = Boolean(anchorEl)

  const handleOpen = (event: React.MouseEvent<HTMLButtonElement>) => setAnchorEl(event.currentTarget)
  const handleClose = () => setAnchorEl(null)

  const changeLanguage = (lng: 'sr' | 'en') => {
    void i18n.changeLanguage(lng)
    handleClose()
  }

  // i18n exposes resolvedLanguage once initialized; fall back to language during SSR-ish edge cases.
  const current = i18n.resolvedLanguage ?? i18n.language

  return (
    <MuiAppBar
      position="fixed"
      elevation={0}
      sx={{
        zIndex: (theme) => theme.zIndex.drawer + 1,
        width: '100%',
        bgcolor: clubNavBlue,
        color: '#fff',
        borderBottom: '1px solid rgba(255,255,255,0.12)',
      }}
    >
      <Toolbar>
        <Box
          component="img"
          src={clubLogoSrc}
          alt={t('app.title')}
          sx={{ height: 44, width: 'auto', mr: 2, display: 'block' }}
        />
        <Typography variant="h6" component="div" sx={{ flexGrow: 1, fontWeight: 600 }}>
          {t('app.title')}
        </Typography>
        <IconButton
          color="inherit"
          aria-label={t('language.label')}
          onClick={handleOpen}
          size="large"
        >
          <LanguageIcon />
        </IconButton>
        <Menu
          anchorEl={anchorEl}
          open={open}
          onClose={handleClose}
          PaperProps={{
            sx: {
              bgcolor: clubNavBlue,
              color: '#fff',
              border: '1px solid rgba(255,255,255,0.2)',
              '& .MuiMenuItem-root': { color: '#fff' },
              '& .MuiMenuItem-root.Mui-selected': { bgcolor: 'rgba(255,255,255,0.12)' },
              '& .MuiMenuItem-root:hover': { bgcolor: 'rgba(255,255,255,0.08)' },
            },
          }}
        >
          <MenuItem onClick={() => changeLanguage('sr')} selected={current === 'sr'}>
            {current === 'sr' ? <CheckIcon fontSize="small" sx={{ mr: 1 }} /> : <span style={{ display: 'inline-block', width: 24 }} />}
            {t('language.sr')}
          </MenuItem>
          <MenuItem onClick={() => changeLanguage('en')} selected={current === 'en'}>
            {current === 'en' ? <CheckIcon fontSize="small" sx={{ mr: 1 }} /> : <span style={{ display: 'inline-block', width: 24 }} />}
            {t('language.en')}
          </MenuItem>
        </Menu>
      </Toolbar>
    </MuiAppBar>
  )
}

export default AppBar
