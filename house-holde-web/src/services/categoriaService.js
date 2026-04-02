import api from './api';
//classe destinada à realizar operações para a entidade categória.
export const categoriaService = {
  async getAll() {
    try {
      const response = await api.get('/categorias');
      // A API retorna um array direto de categorias
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar categorias:', error);
      throw error;
    }
  },

  //lista categórias por finalidade(despesa,receita ou ambas)
  async getByFinalidade(finalidade) {
    try {
      // finalidade como número: 0 = despesa, 1 = receita, 2 = ambas
      const response = await api.get(`/categorias/finalidade/${finalidade}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar categorias por finalidade:', error);
      throw error;
    }
  },

  //cria uma nova categória
  async create(categoria) {
    try {
      // finalidade como número: 0 = despesa, 1 = receita, 2 = ambas
      const categoriaData = {
        ...categoria,
        finalidade: this.mapFinalidadeToNumber(categoria.finalidade)
      };
      
      const response = await api.post('/categorias', categoriaData);
      return response.data;
    } catch (error) {
      console.error('Erro ao criar categoria:', error);
      throw error;
    }
  },

  //deleta uma categória
  async deleteCategorias(id) {
    try {
      const response = await api.delete(`/categorias/${id}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao excluir categória:', error);
      throw error;
    }
  },

  //mapeia uma categória, para índice númerico
  mapFinalidadeToNumber(finalidadeString) {
    const map = {
      'despesa': 0,
      'receita': 1,
      'ambos': 2
    };
    return map[finalidadeString] || 0; // default para despesa
  },

  //mapeia uma categória, para índice string
  mapFinalidadeToString(finalidadeNumber) {
    const map = {
      0: 'Despesa',
      1: 'Receita', 
      2: 'Ambas'
    };
    return map[finalidadeNumber] || 'Despesa';
  }
};