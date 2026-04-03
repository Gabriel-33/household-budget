//classe feita para formatar valores de data e monetário, finalidade e categória
export const formatters = {
  formatCurrency(value) {
    if (value === undefined || value === null) return 'R$ 0,00';
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  },

  formatDate(dateString) {
    if (!dateString) return '';
    try {
      const date = new Date(dateString);
      return new Intl.DateTimeFormat('pt-BR').format(date);
    } catch {
      return dateString;
    }
  },

  formatTipo(tipo) {
    // tipo como número: 0 = despesa, 1 = receita
    if (tipo === 0 || tipo === 'despesa') return 'Despesa';
    if (tipo === 1 || tipo === 'receita') return 'Receita';
    return 'Desconhecido';
  },

  formatFinalidade(finalidade) {
    // finalidade como número: 0 = despesa, 1 = receita, 2 = ambas
    if (finalidade === 0 || finalidade === 'despesa') return 'Despesa';
    if (finalidade === 1 || finalidade === 'receita') return 'Receita';
    if (finalidade === 2 || finalidade === 'ambas') return 'Ambas';
    return 'Desconhecido';
  },

  //mapeia o tipo de despesa do número de uma string para um índice númerico
  mapTipoToNumber(tipoString) {
    const map = {
      'despesa': 0,
      'receita': 1
    };
    return map[tipoString] || 0;
  },

//mapeia o tipo de despesa de um índice númerico para uma string
  mapTipoToString(tipoNumber) {
    const map = {
      0: 'despesa',
      1: 'receita'
    };
    return map[tipoNumber] || 'despesa';
  },

  //retorna a cor ver, caso o tipo seja receita, e vermelho caso seja despesa
  getTipoColor(tipo) {
    if (tipo === 1 || tipo === 'receita') return '#27ae60'; 
    return '#e74c3c'; 
  },

  //retorna a cor ver, caso a finalidade seja receita, vermelho caso seja despesa e azul em caso de ambas
  getFinalidadeColor(finalidade) {
    if (finalidade === 0 || finalidade === 'despesa') return '#e74c3c';
    if (finalidade === 1 || finalidade === 'receita') return '#27ae60';
    return '#3498db'; 
  }
};