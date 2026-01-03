import React, { useState, useEffect } from 'react';
import { categoriaService } from '../../services/categoriaService';
import CategoriaForm from './CategoriaForm';
import { formatters } from '../../utils/formatters';

const CategoriasList = () => {
  const [categorias, setCategorias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [filter, setFilter] = useState('todas');

  const loadCategorias = async () => {
    try {
      setLoading(true);
      const data = await categoriaService.getAll();
      setCategorias(data || []);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar categorias');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCategorias();
  }, []);

  const handleFormSuccess = () => {
    setShowForm(false);
    loadCategorias();
    alert('Categoria cadastrada com sucesso!');
  };

  const filteredCategorias = filter === 'todas' 
    ? categorias 
    : categorias.filter(cat => {
        const finalidadeNum = parseInt(filter);
        return cat.finalidade === finalidadeNum;
      });

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
    <div className="categorias-container">
      <div className="header">
        <h2>Cadastro de Categorias</h2>
        <button 
          onClick={() => setShowForm(true)}
          className="btn-primary"
        >
          + Nova Categoria
        </button>
      </div>

      {showForm ? (
        <CategoriaForm 
          onSuccess={handleFormSuccess}
          onCancel={() => setShowForm(false)}
        />
      ) : (
        <>
          <div className="filter-section">
            <label>Filtrar por finalidade:</label>
            <select 
              value={filter} 
              onChange={(e) => setFilter(e.target.value)}
              className="filter-select"
            >
              <option value="todas">Todas</option>
              <option value="0">Despesa</option>
              <option value="1">Receita</option>
              <option value="2">Ambas</option>
            </select>
          </div>

          {filteredCategorias.length === 0 ? (
            <div className="empty-state">
              <p>Nenhuma categoria encontrada.</p>
            </div>
          ) : (
            <div className="categorias-grid">
              {filteredCategorias.map(categoria => (
                <div key={categoria.id} className="categoria-card">
                  <div className="categoria-header">
                    <h3>{categoria.descricao}</h3>
                    <span 
                      className="badge"
                      style={{ 
                        backgroundColor: formatters.getFinalidadeColor(categoria.finalidade) 
                      }}
                    >
                      {formatters.formatFinalidade(categoria.finalidade)}
                    </span>
                  </div>
                  <div className="categoria-info">
                    <p><strong>ID:</strong> {categoria.id}</p>
                    <p><strong>Finalidade:</strong> {formatters.formatFinalidade(categoria.finalidade)}</p>
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

export default CategoriasList;