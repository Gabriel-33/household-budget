//classe que contém constantes utilizadas no sistema
export const constants = {
  API_URL: process.env.REACT_APP_API_URL || 'localhost',
  API_KEY: 'api-key-da-api',

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