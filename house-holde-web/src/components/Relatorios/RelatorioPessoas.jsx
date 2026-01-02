import React, { useState, useEffect } from 'react';
import { relatorioService } from '../../services/relatorioService';
import { pessoaService } from '../../services/pessoaService';
import { formatters } from '../../utils/formatters';

const RelatorioPessoas = () => {
  const [relatorioData, setRelatorioData] = useState([]);
  const [pessoas, setPessoas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [ordenacao, setOrdenacao] = useState('nome');

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Faz as requisições em paralelo
      const [relatorioResponse, pessoasResponse] = await Promise.all([
        relatorioService.getRelatorioPessoas(),
        pessoaService.getAll()
      ]);
      
      console.log('Resposta do relatório:', relatorioResponse);
      console.log('Resposta das pessoas:', pessoasResponse);
      
      // Processa os dados do relatório
      let relatorioProcessado = [];
      
      if (Array.isArray(relatorioResponse)) {
        // Se for array direto
        relatorioProcessado = relatorioResponse;
      } else if (relatorioResponse && typeof relatorioResponse === 'object') {
        // Se for objeto com propriedade items ou transacoes
        if (relatorioResponse.items && Array.isArray(relatorioResponse.items)) {
          relatorioProcessado = relatorioResponse.items;
        } else if (relatorioResponse.transacoes && Array.isArray(relatorioResponse.transacoes)) {
          relatorioProcessado = relatorioResponse.transacoes;
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
      setPessoas(pessoasResponse.pessoas || []);
      
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

  const getPessoaInfo = (pessoaId) => {
    const pessoa = pessoas.find(p => p.id === pessoaId);
    return pessoa || { nome: 'Desconhecida', idade: 0 };
  };

  // Garante que relatorioData é um array para ordenação
  const relatorioOrdenado = Array.isArray(relatorioData) 
    ? [...relatorioData].sort((a, b) => {
        const pessoaA = getPessoaInfo(a.pessoaId || a.id);
        const pessoaB = getPessoaInfo(b.pessoaId || b.id);
        
        switch(ordenacao) {
          case 'nome':
            return (pessoaA.nome || '').localeCompare(pessoaB.nome || '');
          case 'idade':
            return (pessoaB.idade || 0) - (pessoaA.idade || 0);
          case 'receitas':
            return (b.totalReceitas || 0) - (a.totalReceitas || 0);
          case 'despesas':
            return (b.totalDespesas || 0) - (a.totalDespesas || 0);
          case 'saldo':
            const saldoA = (a.totalReceitas || 0) - (a.totalDespesas || 0);
            const saldoB = (b.totalReceitas || 0) - (b.totalDespesas || 0);
            return saldoB - saldoA;
          default:
            return 0;
        }
      })
    : [];

  const totalGeralReceitas = relatorioOrdenado.reduce(
    (sum, item) => sum + (item.totalReceitas || 0), 0
  );
  
  const totalGeralDespesas = relatorioOrdenado.reduce(
    (sum, item) => sum + (item.totalDespesas || 0), 0
  );
  
  const saldoGeral = totalGeralReceitas - totalGeralDespesas;

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
        <h2>Relatório por Pessoa</h2>
        <div className="header-actions">
          <select 
            value={ordenacao} 
            onChange={(e) => setOrdenacao(e.target.value)}
            className="select-ordenacao"
            disabled={relatorioOrdenado.length === 0}
          >
            <option value="nome">Ordenar por Nome</option>
            <option value="idade">Ordenar por Idade</option>
            <option value="receitas">Ordenar por Receitas</option>
            <option value="despesas">Ordenar por Despesas</option>
            <option value="saldo">Ordenar por Saldo</option>
          </select>
          <button onClick={loadData} className="btn-secondary">
            Atualizar
          </button>
        </div>
      </div>

      <div className="resumo-geral">
        <div className="resumo-card">
          <h3>Total Receitas</h3>
          <p className="valor positivo">{formatters.formatCurrency(totalGeralReceitas)}</p>
        </div>
        <div className="resumo-card">
          <h3>Total Despesas</h3>
          <p className="valor negativo">{formatters.formatCurrency(totalGeralDespesas)}</p>
        </div>
        <div className="resumo-card">
          <h3>Saldo Geral</h3>
          <p className={`valor ${saldoGeral >= 0 ? 'positivo' : 'negativo'}`}>
            {formatters.formatCurrency(saldoGeral)}
          </p>
        </div>
      </div>

      {relatorioOrdenado.length === 0 ? (
        <div className="empty-state">
          <p>Nenhum dado disponível para o relatório.</p>
          {relatorioData.length === 0 ? (
            <p className="empty-state-hint">
              Aguarde até que existam transações cadastradas.
            </p>
          ) : (
            <p className="empty-state-hint">
              Estrutura de dados não reconhecida. Verifique o console para debug.
            </p>
          )}
        </div>
      ) : (
        <div className="relatorio-table">
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Idade</th>
                <th>Total Receitas</th>
                <th>Total Despesas</th>
                <th>Saldo</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {relatorioOrdenado.map((item, index) => {
                const pessoaId = item.pessoaId || item.id;
                const pessoa = getPessoaInfo(pessoaId);
                const totalReceitas = item.totalReceitas || 0;
                const totalDespesas = item.totalDespesas || 0;
                const saldo = totalReceitas - totalDespesas;
                const status = saldo >= 0 ? 'positivo' : 'negativo';
                
                return (
                  <tr key={item.id || `pessoa-${index}`}>
                    <td>
                      <strong>{pessoa.nome || `Pessoa ${pessoaId}`}</strong>
                      <small>ID: {pessoaId}</small>
                    </td>
                    <td>{pessoa.idade || 'N/A'} anos</td>
                    <td className="positivo">{formatters.formatCurrency(totalReceitas)}</td>
                    <td className="negativo">{formatters.formatCurrency(totalDespesas)}</td>
                    <td className={status}>{formatters.formatCurrency(saldo)}</td>
                    <td>
                      <span className={`status-badge ${status}`}>
                        {status === 'positivo' ? 'Positivo' : 'Negativo'}
                      </span>
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

export default RelatorioPessoas;