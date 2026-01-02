import api from './api';

export const transacaoService = {
  async getAll(page = 1, pageSize = 50) {
    try {
      const response = await api.get('/transacoes', {
        params: { page, pageSize }
      });
      
      // A API retorna um objeto com transacoes array e metadata
      return {
        transacoes: response.data.transacoes || [],
        pagination: {
          maxPage: response.data.maxPage || 1,
          totalCount: response.data.totalCount || 0,
          pageCount: response.data.pageCount || 0,
          currentPage: page
        }
      };
    } catch (error) {
      console.error('Erro ao buscar transações:', error);
      throw error;
    }
  },

  async create(transacao) {
    try {
      // A API espera tipo como número: 0 = despesa, 1 = receita
      const transacaoData = {
        ...transacao,
        tipo: transacao.tipo === 'receita' ? 1 : 0
      };
      
      const response = await api.post('/transacoes', transacaoData);
      return response.data;
    } catch (error) {
      console.error('Erro ao criar transação:', error);
      throw error;
    }
  }
};