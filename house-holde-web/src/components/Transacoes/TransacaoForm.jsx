import React, { useState, useEffect } from 'react';
import { transacaoService } from '../../services/transacaoService';
import { pessoaService } from '../../services/pessoaService';
import { categoriaService } from '../../services/categoriaService';
import { formatters } from '../../utils/formatters';

const TransacaoForm = ({ onSuccess, onCancel, pessoas: pessoasProp, categorias: categoriasProp }) => {
  const [formData, setFormData] = useState({
    descricao: '',
    valor: '',
    tipo: 'despesa', // string: 'despesa' ou 'receita'
    categoriaId: '',
    pessoaId: ''
  });
  
  const [pessoas, setPessoas] = useState(pessoasProp || []);
  const [categorias, setCategorias] = useState(categoriasProp || []);
  const [filteredCategorias, setFilteredCategorias] = useState([]);
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [loading, setLoading] = useState(!pessoasProp || !categoriasProp);

  useEffect(() => {
    const loadData = async () => {
      if (!pessoasProp || !categoriasProp) {
        try {
          setLoading(true);
          const [pessoasData, categoriasData] = await Promise.all([
            pessoaService.getAll(),
            categoriaService.getAll()
          ]);
          
          setPessoas(pessoasData.pessoas || []);
          setCategorias(categoriasData || []);
        } catch (error) {
          console.error('Erro ao carregar dados:', error);
        } finally {
          setLoading(false);
        }
      }
    };

    loadData();
  }, [pessoasProp, categoriasProp]);

  useEffect(() => {
    // Filtra categorias baseado no tipo selecionado
    if (formData.tipo) {
      const tipoNumber = formatters.mapTipoToNumber(formData.tipo);
      const filtered = categorias.filter(categoria => {
        // Categorias com finalidade 2 (ambas) servem para ambos os tipos
        if (categoria.finalidade === 2) return true;
        // Para despesa (0), só categorias com finalidade 0 (despesa)
        if (tipoNumber === 0) return categoria.finalidade === 0;
        // Para receita (1), só categorias com finalidade 1 (receita)
        if (tipoNumber === 1) return categoria.finalidade === 1;
        return false;
      });
      setFilteredCategorias(filtered);
      
      // Se a categoria selecionada não está nas filtradas, limpa a seleção
      if (formData.categoriaId && !filtered.some(c => c.id === parseInt(formData.categoriaId))) {
        setFormData(prev => ({ ...prev, categoriaId: '' }));
      }
    }
  }, [formData.tipo, categorias]);

  useEffect(() => {
    // Verifica se a pessoa selecionada é menor de idade
    if (formData.pessoaId) {
      const pessoaSelecionada = pessoas.find(p => p.id === parseInt(formData.pessoaId));
      if (pessoaSelecionada && pessoaSelecionada.idade < 18) {
        setFormData(prev => ({ ...prev, tipo: 'despesa' }));
      }
    }
  }, [formData.pessoaId, pessoas]);

  const validateForm = () => {
    const newErrors = {};
    
    if (!formData.descricao.trim()) {
      newErrors.descricao = 'Descrição é obrigatória';
    }
    
    const valorNum = parseFloat(formData.valor);
    if (!formData.valor || isNaN(valorNum) || valorNum <= 0) {
      newErrors.valor = 'Valor deve ser um número positivo';
    }
    
    if (!formData.categoriaId) {
      newErrors.categoriaId = 'Categoria é obrigatória';
    }
    
    if (!formData.pessoaId) {
      newErrors.pessoaId = 'Pessoa é obrigatória';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    
    setFormData(prev => ({ ...prev, [name]: value }));
    
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    try {
      // Converte string para números
      const transacaoData = {
        descricao: formData.descricao,
        valor: parseFloat(formData.valor),
        tipo: formData.tipo, // Já está como string 'despesa' ou 'receita'
        categoriaId: parseInt(formData.categoriaId),
        pessoaId: parseInt(formData.pessoaId)
      };
      
      await transacaoService.create(transacaoData);
      onSuccess();
    } catch (error) {
      console.error('Erro ao criar transação:', error);
      
      let errorMessage = 'Erro ao criar transação.';
      if (error.response?.data?.errors) {
        errorMessage = Object.values(error.response.data.errors).flat().join(', ');
      }
      
      alert(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Carregando dados...</p>
      </div>
    );
  }

  return (
    <div className="transacao-form">
      <h3>Cadastrar Nova Transação</h3>
      
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="descricao">Descrição *</label>
          <input
            type="text"
            id="descricao"
            name="descricao"
            value={formData.descricao}
            onChange={handleChange}
            className={errors.descricao ? 'error' : ''}
            placeholder="Digite a descrição da transação"
            disabled={isSubmitting}
          />
          {errors.descricao && <span className="error-text">{errors.descricao}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="valor">Valor *</label>
          <input
            type="number"
            id="valor"
            name="valor"
            value={formData.valor}
            onChange={handleChange}
            className={errors.valor ? 'error' : ''}
            placeholder="0.00"
            min="0.01"
            step="0.01"
            disabled={isSubmitting}
          />
          {errors.valor && <span className="error-text">{errors.valor}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="pessoaId">Pessoa *</label>
          <select
            id="pessoaId"
            name="pessoaId"
            value={formData.pessoaId}
            onChange={handleChange}
            className={errors.pessoaId ? 'error' : ''}
            disabled={isSubmitting || pessoas.length === 0}
          >
            <option value="">Selecione uma pessoa</option>
            {pessoas.map(pessoa => (
              <option key={pessoa.id} value={pessoa.id}>
                {pessoa.nome} ({pessoa.idade} anos)
              </option>
            ))}
          </select>
          {errors.pessoaId && <span className="error-text">{errors.pessoaId}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="tipo">Tipo *</label>
          <select
            id="tipo"
            name="tipo"
            value={formData.tipo}
            onChange={handleChange}
            disabled={isSubmitting || (formData.pessoaId && pessoas.find(p => p.id === parseInt(formData.pessoaId))?.idade < 18)}
          >
            <option value="despesa">Despesa</option>
            <option value="receita">Receita</option>
          </select>
          {formData.pessoaId && pessoas.find(p => p.id === parseInt(formData.pessoaId))?.idade < 18 && (
            <small className="info-text">⚠️ Menores de idade só podem ter despesas</small>
          )}
        </div>
        
        <div className="form-group">
          <label htmlFor="categoriaId">Categoria *</label>
          <select
            id="categoriaId"
            name="categoriaId"
            value={formData.categoriaId}
            onChange={handleChange}
            className={errors.categoriaId ? 'error' : ''}
            disabled={isSubmitting || filteredCategorias.length === 0}
          >
            <option value="">Selecione uma categoria</option>
            {filteredCategorias.map(categoria => (
              <option key={categoria.id} value={categoria.id}>
                {categoria.descricao} ({formatters.formatFinalidade(categoria.finalidade)})
              </option>
            ))}
          </select>
          {errors.categoriaId && <span className="error-text">{errors.categoriaId}</span>}
          {filteredCategorias.length === 0 && formData.tipo && (
            <small className="info-text">
              Nenhuma categoria disponível para o tipo selecionado
            </small>
          )}
        </div>
        
        <div className="form-actions">
          <button 
            type="button" 
            onClick={onCancel}
            className="btn-secondary"
            disabled={isSubmitting}
          >
            Cancelar
          </button>
          <button 
            type="submit" 
            className="btn-primary"
            disabled={isSubmitting}
          >
            {isSubmitting ? 'Salvando...' : 'Salvar'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default TransacaoForm;