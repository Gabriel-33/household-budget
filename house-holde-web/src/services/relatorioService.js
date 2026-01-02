import api from './api';

export const relatorioService = {
  async getRelatorioPessoas() {
    try {
      const response = await api.get('/relatorios/pessoas');
      // Ajuste conforme a estrutura real da API
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar relatório de pessoas:', error);
      throw error;
    }
  },

  async getRelatorioCategorias() {
    try {
      const response = await api.get('/relatorios/categorias');
      // Ajuste conforme a estrutura real da API
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar relatório de categorias:', error);
      throw error;
    }
  }
};