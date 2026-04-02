import api from './api';

//classe destinada à serviços da entidade pessoa.
export const pessoaService = {
  async getAll(page = 1, pageSize = 50) {
    try {
      const response = await api.get('/pessoas', {
        params: { page, pageSize }
      });
      
      // A API retorna um objeto com items array e metadata
      return {
        pessoas: response.data.items || [],
        pagination: {
          maxPage: response.data.maxPage || 1,
          totalCount: response.data.totalCount || 0,
          pageCount: response.data.pageCount || 0,
          currentPage: page
        }
      };
    } catch (error) {
      console.error('Erro ao buscar pessoas:', error);
      throw error;
    }
  },

  //cadastra uma pessoa no sistema
  async create(pessoa) {
    try {
      const response = await api.post('/pessoas', pessoa);
      return response.data;
    } catch (error) {
      console.error('Erro ao criar pessoa:', error);
      throw error;
    }
  },

  //exclui uma pessoa do sistema
  async delete(id) {
    try {
      const response = await api.delete(`/pessoas/${id}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao excluir pessoa:', error);
      throw error;
    }
  }
};