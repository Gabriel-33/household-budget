import api from './api';

//classe destinada à trazer relatórios de transações, por pessoa e categória
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

  //retorna transações por categória
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