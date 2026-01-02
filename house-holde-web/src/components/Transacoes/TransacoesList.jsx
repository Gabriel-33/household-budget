import React, { useState, useEffect } from 'react';
import { transacaoService } from '../../services/transacaoService';
import { pessoaService } from '../../services/pessoaService';
import { categoriaService } from '../../services/categoriaService';
import TransacaoForm from './TransacaoForm';
import { formatters } from '../../utils/formatters';

const TransacoesList = () => {
  const [data, setData] = useState({
    transacoes: [],
    pessoas: [],
    categorias: []
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [filters, setFilters] = useState({
    pessoaId: '',
    tipo: '',
    categoriaId: ''
  });

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Faz todas as requisições em paralelo
      const [transacoesData, pessoasData, categoriasData] = await Promise.all([
        transacaoService.getAll(),
        pessoaService.getAll(),
        categoriaService.getAll()
      ]);
      
      setData({
        transacoes: transacoesData.transacoes || [],
        pessoas: pessoasData.pessoas || [],
        categorias: categoriasData || []
      });
      
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
      setError('Erro ao carregar dados. Tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const handleFormSuccess = () => {
    setShowForm(false);
    loadData();
  };

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const clearFilters = () => {
    setFilters({
      pessoaId: '',
      tipo: '',
      categoriaId: ''
    });
  };

  // Filtra as transações
  const filteredTransacoes = data.transacoes.filter(transacao => {
    if (filters.pessoaId && transacao.pessoaNome !== filters.pessoaId) return false;
    if (filters.tipo && transacao.tipo !== parseInt(filters.tipo)) return false;
    if (filters.categoriaId && transacao.categoriaDescricao !== filters.categoriaId) return false;
    return true;
  });

  // Cálculos totais
  const totalReceitas = filteredTransacoes
    .filter(t => t.tipo === 1) // 1 = receita
    .reduce((sum, t) => sum + (t.valor || 0), 0);

  const totalDespesas = filteredTransacoes
    .filter(t => t.tipo === 0) // 0 = despesa
    .reduce((sum, t) => sum + (t.valor || 0), 0);

  const saldo = totalReceitas - totalDespesas;

  // Extrair pessoas únicas para filtro
  const pessoasUnicas = Array.from(
    new Set(data.transacoes.map(t => t.pessoaNome))
  ).filter(Boolean);

  // Extrair categorias únicas para filtro
  const categoriasUnicas = Array.from(
    new Set(data.transacoes.map(t => t.categoriaDescricao))
  ).filter(Boolean);

  if (loading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Carregando transações...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <div className="error-icon">⚠️</div>
        <h3>Erro ao carregar dados</h3>
        <p>{error}</p>
        <button onClick={loadData} className="btn-primary">
          Tentar novamente
        </button>
      </div>
    );
  }

  return (
    <div className="transacoes-container">
      <div className="header">
        <h2>Transações</h2>
        <button 
          onClick={() => setShowForm(true)}
          className="btn-primary"
          disabled={loading}
        >
          + Nova Transação
        </button>
      </div>

      {showForm ? (
        <TransacaoForm 
          onSuccess={handleFormSuccess}
          onCancel={() => setShowForm(false)}
          pessoas={data.pessoas}
          categorias={data.categorias}
        />
      ) : (
        <>
          {/* Resumo Financeiro */}
          <div className="resumo-financeiro">
            <div className="resumo-card">
              <h3>Receitas</h3>
              <p className="valor positivo">{formatters.formatCurrency(totalReceitas)}</p>
            </div>
            <div className="resumo-card">
              <h3>Despesas</h3>
              <p className="valor negativo">{formatters.formatCurrency(totalDespesas)}</p>
            </div>
            <div className="resumo-card">
              <h3>Saldo</h3>
              <p className={`valor ${saldo >= 0 ? 'positivo' : 'negativo'}`}>
                {formatters.formatCurrency(saldo)}
              </p>
            </div>
          </div>

          {/* Filtros */}
          <div className="filtros-section">
            <h3>Filtros</h3>
            <div className="filtros-row">
              <div className="filter-group">
                <label>Pessoa</label>
                <select 
                  name="pessoaId"
                  value={filters.pessoaId}
                  onChange={handleFilterChange}
                  disabled={pessoasUnicas.length === 0}
                >
                  <option value="">Todas as pessoas</option>
                  {pessoasUnicas.map(pessoa => (
                    <option key={pessoa} value={pessoa}>
                      {pessoa}
                    </option>
                  ))}
                </select>
              </div>

              <div className="filter-group">
                <label>Tipo</label>
                <select 
                  name="tipo"
                  value={filters.tipo}
                  onChange={handleFilterChange}
                >
                  <option value="">Todos os tipos</option>
                  <option value="1">Receita</option>
                  <option value="0">Despesa</option>
                </select>
              </div>

              <div className="filter-group">
                <label>Categoria</label>
                <select 
                  name="categoriaId"
                  value={filters.categoriaId}
                  onChange={handleFilterChange}
                  disabled={categoriasUnicas.length === 0}
                >
                  <option value="">Todas as categorias</option>
                  {categoriasUnicas.map(categoria => (
                    <option key={categoria} value={categoria}>
                      {categoria}
                    </option>
                  ))}
                </select>
              </div>

              <button 
                onClick={clearFilters}
                className="btn-secondary"
                disabled={!filters.pessoaId && !filters.tipo && !filters.categoriaId}
              >
                Limpar Filtros
              </button>
            </div>
          </div>

          {/* Lista de Transações */}
          {filteredTransacoes.length === 0 ? (
            <div className="empty-state">
              {data.transacoes.length === 0 ? (
                <>
                  <p>Nenhuma transação cadastrada ainda.</p>
                  <button 
                    onClick={() => setShowForm(true)}
                    className="btn-primary"
                  >
                    Criar primeira transação
                  </button>
                </>
              ) : (
                <>
                  <p>Nenhuma transação encontrada com os filtros atuais.</p>
                  <button 
                    onClick={clearFilters}
                    className="btn-secondary"
                  >
                    Limpar filtros
                  </button>
                </>
              )}
            </div>
          ) : (
            <div className="transacoes-list">
              <div className="transacoes-header">
                <span className="total-transacoes">
                  {filteredTransacoes.length} transação{filteredTransacoes.length !== 1 ? 's' : ''}
                </span>
              </div>
              
              {filteredTransacoes.map(transacao => (
                <div key={transacao.id} className="transacao-card">
                  <div className="transacao-header">
                    <div className="transacao-info">
                      <h3>{transacao.descricao}</h3>
                      <p className={`tipo-badge ${transacao.tipo === 1 ? 'receita' : 'despesa'}`}>
                        {formatters.formatTipo(transacao.tipo)}
                      </p>
                    </div>
                    <div className="transacao-valor">
                      <span className={`valor ${transacao.tipo === 1 ? 'receita' : 'despesa'}`}>
                        {transacao.tipo === 1 ? '+' : '-'}
                        {formatters.formatCurrency(transacao.valor || 0)}
                      </span>
                      <small className="transacao-data">
                        {formatters.formatDate(transacao.data)}
                      </small>
                    </div>
                  </div>
                  
                  <div className="transacao-detalhes">
                    <div className="detalhe">
                      <span className="detalhe-label">Pessoa:</span>
                      <span className="detalhe-valor">{transacao.pessoaNome}</span>
                    </div>
                    <div className="detalhe">
                      <span className="detalhe-label">Categoria:</span>
                      <span className="detalhe-valor">{transacao.categoriaDescricao}</span>
                    </div>
                    <div className="detalhe">
                      <span className="detalhe-label">ID:</span>
                      <span className="detalhe-valor">{transacao.id}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default TransacoesList;