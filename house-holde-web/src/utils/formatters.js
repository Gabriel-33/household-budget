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

  mapTipoToNumber(tipoString) {
    const map = {
      'despesa': 0,
      'receita': 1
    };
    return map[tipoString] || 0;
  },

  mapTipoToString(tipoNumber) {
    const map = {
      0: 'despesa',
      1: 'receita'
    };
    return map[tipoNumber] || 'despesa';
  },

  getTipoColor(tipo) {
    if (tipo === 1 || tipo === 'receita') return '#27ae60'; // verde
    return '#e74c3c'; // vermelho
  },

  getFinalidadeColor(finalidade) {
    if (finalidade === 0 || finalidade === 'despesa') return '#e74c3c';
    if (finalidade === 1 || finalidade === 'receita') return '#27ae60';
    return '#3498db'; // azul para ambas
  }
};