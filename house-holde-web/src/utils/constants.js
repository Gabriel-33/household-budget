export const constants = {
  API_URL: process.env.REACT_APP_API_URL || 'http://localhost:3000/api',
  
  TIPOS_TRANSACAO: [
    { value: 'despesa', label: 'Despesa' },
    { value: 'receita', label: 'Receita' }
  ],
  
  FINALIDADES_CATEGORIA: [
    { value: 'despesa', label: 'Despesa' },
    { value: 'receita', label: 'Receita' },
    { value: 'ambas', label: 'Ambas' }
  ],
  
  IDADE_MINIMA_TRANSACAO: 18
};