import React, { useState, useEffect } from 'react';
import { pessoaService } from '../../services/pessoaService';
import PessoaCard from './PessoaCard';
import PessoaForm from './PessoaForm';

const PessoasList = () => {
  const [data, setData] = useState({
    pessoas: [],
    pagination: {}
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);

  const loadPessoas = async () => {
    try {
      setLoading(true);
      const response = await pessoaService.getAll();
      setData({
        pessoas: response.pessoas || [],
        pagination: response.pagination || {}
      });
      setError(null);
    } catch (err) {
      setError('Erro ao carregar pessoas');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPessoas();
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm('Tem certeza que deseja excluir esta pessoa? Todas as transações associadas também serão excluídas.')) {
      try {
        await pessoaService.delete(id);
        await loadPessoas();
        alert('Pessoa excluída com sucesso!');
      } catch (error) {
        alert('Erro ao excluir pessoa');
        console.error(error);
      }
    }
  };

  const handleFormSuccess = () => {
    setShowForm(false);
    loadPessoas();
    alert('Pessoa cadastrada com sucesso!');
  };

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
      </div>
    );
  }

  return (
    <div className="pessoas-container">
      <div className="header">
        <h2>Cadastro de Pessoas</h2>
        <button 
          onClick={() => setShowForm(true)}
          className="btn-primary"
        >
          + Nova Pessoa
        </button>
      </div>
      
      {showForm ? (
        <PessoaForm 
          onSuccess={handleFormSuccess}
          onCancel={() => setShowForm(false)}
        />
      ) : (
        <>
          <div className="pessoas-info">
            <p>Total de pessoas: {data.pagination.totalCount || data.pessoas.length}</p>
          </div>
          
          {data.pessoas.length === 0 ? (
            <div className="empty-state">
              <p>Nenhuma pessoa cadastrada ainda.</p>
              <button 
                onClick={() => setShowForm(true)}
                className="btn-secondary"
              >
                Cadastrar primeira pessoa
              </button>
            </div>
          ) : (
            <div className="pessoas-grid">
              {data.pessoas.map(pessoa => (
                <PessoaCard 
                  key={pessoa.id} 
                  pessoa={pessoa}
                  onDelete={handleDelete}
                />
              ))}
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default PessoasList;