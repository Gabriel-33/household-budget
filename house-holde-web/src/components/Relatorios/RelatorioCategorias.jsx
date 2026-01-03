import React, { useState, useEffect } from 'react';
import { relatorioService } from '../../services/relatorioService';
import { categoriaService } from '../../services/categoriaService';
import { formatters } from '../../utils/formatters';

const RelatorioCategorias = () => {
  const [relatorioData, setRelatorioData] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filtroTipo, setFiltroTipo] = useState('todas');
  const [ordenacao, setOrdenacao] = useState('total');

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const [relatorioResponse, categoriasData] = await Promise.all([
        relatorioService.getRelatorioCategorias(),
        categoriaService.getAll()
      ]);
      
      console.log('Resposta do relatório categorias:', relatorioResponse);
      
      // Processa os dados do relatório
      let relatorioProcessado = [];
      
      if (Array.isArray(relatorioResponse)) {
        relatorioProcessado = relatorioResponse;
      } else if (relatorioResponse && typeof relatorioResponse === 'object') {
        if (relatorioResponse.items && Array.isArray(relatorioResponse.items)) {
          relatorioProcessado = relatorioResponse.items;
        } else if (relatorioResponse.data && Array.isArray(relatorioResponse.data)) {
          relatorioProcessado = relatorioResponse.data;
        } else {
          // Tenta extrair arrays das propriedades do objeto
          const possibleArrays = Object.values(relatorioResponse).filter(
            value => Array.isArray(value)
          );
          if (possibleArrays.length > 0) {
            relatorioProcessado = possibleArrays[0];
          }
        }
      }
      
      setRelatorioData(relatorioProcessado);
      setCategorias(categoriasData || []);
      
    } catch (err) {
      console.error('Erro ao carregar relatório:', err);
      setError('Erro ao carregar relatório. Tente novamente mais tarde.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const getCategoriaInfo = (categoriaId) => {
    const categoria = categorias.find(c => c.id === categoriaId);
    return categoria || { descricao: 'Desconhecida', finalidade: 2 };
  };

  // Filtra por tipo de transação (se aplicável)
  const relatorioFiltrado = Array.isArray(relatorioData) 
    ? relatorioData.filter(item => {
        if (filtroTipo === 'todas') return true;
        
        const categoriaId = item.categoriaId || item.id;
        const categoria = getCategoriaInfo(categoriaId);
        const finalidadeNum = parseInt(filtroTipo);
        
        if (finalidadeNum === 0) return categoria.finalidade === 0 || categoria.finalidade === 2;
        if (finalidadeNum === 1) return categoria.finalidade === 1 || categoria.finalidade === 2;
        return true;
      })
    : [];

  const relatorioOrdenado = [...relatorioFiltrado].sort((a, b) => {
    const categoriaA = getCategoriaInfo(a.categoriaId || a.id);
    const categoriaB = getCategoriaInfo(b.categoriaId || b.id);
    
    switch(ordenacao) {
      case 'descricao':
        return (categoriaA.descricao || '').localeCompare(categoriaB.descricao || '');
      case 'finalidade':
        return (categoriaA.finalidade || 2) - (categoriaB.finalidade || 2);
      case 'total':
        const totalA = (a.totalReceitas || 0) + (a.totalDespesas || 0);
        const totalB = (b.totalReceitas || 0) + (b.totalDespesas || 0);
        return totalB - totalA;
      case 'receitas':
        return (b.totalReceitas || 0) - (a.totalReceitas || 0);
      case 'despesas':
        return (b.totalDespesas || 0) - (a.totalDespesas || 0);
      default:
        return 0;
    }
  });

  const totalGeral = relatorioOrdenado.reduce(
    (sum, item) => sum + (item.totalReceitas || 0) + (item.totalDespesas || 0), 0
  );
  
  const totalReceitas = relatorioOrdenado.reduce(
    (sum, item) => sum + (item.totalReceitas || 0), 0
  );
  
  const totalDespesas = relatorioOrdenado.reduce(
    (sum, item) => sum + (item.totalDespesas || 0), 0
  );

  if (loading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Carregando relatório...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <div className="error-icon">⚠️</div>
        <h3>Erro ao carregar relatório</h3>
        <p>{error}</p>
        <button onClick={loadData} className="btn-primary">
          Tentar novamente
        </button>
      </div>
    );
  }

  return (
    <div className="relatorio-container">
      <div className="header">
        <h2>Relatório por Categoria</h2>
        <div className="header-actions">
          <div className="filtros-relatorio">
            <select 
              value={filtroTipo} 
              onChange={(e) => setFiltroTipo(e.target.value)}
              className="select-ordenacao"
              disabled={relatorioFiltrado.length === 0}
            >
              <option value="todas">Todas as categorias</option>
              <option value="0">Apenas despesas</option>
              <option value="1">Apenas receitas</option>
            </select>
            
            <select 
              value={ordenacao} 
              onChange={(e) => setOrdenacao(e.target.value)}
              className="select-ordenacao"
              disabled={relatorioFiltrado.length === 0}
            >
              <option value="descricao">Ordenar por Descrição</option>
              <option value="finalidade">Ordenar por Finalidade</option>
              <option value="total">Ordenar por Total</option>
              <option value="receitas">Ordenar por Receitas</option>
              <option value="despesas">Ordenar por Despesas</option>
            </select>
            
          </div>
          <button onClick={loadData} className="btn-secondary">
              Atualizar
            </button>
        </div>
      </div>

      <div className="resumo-geral">
        <div className="resumo-card">
          <h3>Total Movimentado</h3>
          <p className="valor">{formatters.formatCurrency(totalGeral)}</p>
        </div>
        <div className="resumo-card">
          <h3>Total Receitas</h3>
          <p className="valor positivo">{formatters.formatCurrency(totalReceitas)}</p>
        </div>
        <div className="resumo-card">
          <h3>Total Despesas</h3>
          <p className="valor negativo">{formatters.formatCurrency(totalDespesas)}</p>
        </div>
      </div>

      {relatorioOrdenado.length === 0 ? (
        <div className="empty-state">
          <p>Nenhum dado disponível para o relatório.</p>
          <button onClick={loadData} className="btn-secondary">
            Recarregar
          </button>
        </div>
      ) : (
        <div className="relatorio-table">
          <table>
            <thead>
              <tr>
                <th>Categoria</th>
                <th>Finalidade</th>
                <th>Total Receitas</th>
                <th>Total Despesas</th>
                <th>Total Geral</th>
                <th>Porcentagem</th>
              </tr>
            </thead>
            <tbody>
              {relatorioOrdenado.map((item, index) => {
                const categoriaId = item.categoriaId || item.id;
                const categoria = getCategoriaInfo(categoriaId);
                const totalReceitasItem = item.totalReceitas || 0;
                const totalDespesasItem = item.totalDespesas || 0;
                const totalGeralItem = totalReceitasItem + totalDespesasItem;
                const porcentagem = totalGeral > 0 
                  ? ((totalGeralItem / totalGeral) * 100).toFixed(1) 
                  : '0.0';
                
                return (
                  <tr key={item.id || `categoria-${index}`}>
                    <td>
                      <strong>{categoria.descricao || `Categoria ${categoriaId}`}</strong>
                      <small>ID: {categoriaId}</small>
                    </td>
                    <td>
                      <span 
                        className="badge"
                        style={{ 
                          backgroundColor: formatters.getFinalidadeColor(categoria.finalidade) 
                        }}
                      >
                        {formatters.formatFinalidade(categoria.finalidade)}
                      </span>
                    </td>
                    <td className="positivo">{formatters.formatCurrency(totalReceitasItem)}</td>
                    <td className="negativo">{formatters.formatCurrency(totalDespesasItem)}</td>
                    <td className="valor-total">{formatters.formatCurrency(totalGeralItem)}</td>
                    <td>
                      <div className="porcentagem-container">
                        <div className="porcentagem-bar">
                          <div 
                            className="porcentagem-fill"
                            style={{ width: `${Math.min(100, parseFloat(porcentagem))}%` }}
                          ></div>
                        </div>
                        <span className="porcentagem-text">{porcentagem}%</span>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default RelatorioCategorias;