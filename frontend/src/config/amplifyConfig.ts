const amplifyConfig = {
  Auth: {
    Cognito: {
      userPoolId: import.meta.env.VITE_AWS_COGNITO_USER_POOL_ID || '',
      userPoolClientId: import.meta.env.VITE_AWS_COGNITO_CLIENT_ID || '',
      region: import.meta.env.VITE_AWS_REGION || 'us-east-1',
    },
  },
}

export default amplifyConfig

