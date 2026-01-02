import React, { useState } from 'react';
import { categoriaService } from '../../services/categoriaService';

const CategoriaForm = ({ onSuccess, onCancel }) => {
  const [formData, setFormData] = useState({
    descricao: '',
    finalidade: 'ambas'
  });
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const finalidades = [
    { value: 'despesa', label: 'Despesa' },
    { value: 'receita', label: 'Receita' },
    { value: 'ambas', label: 'Ambas' }
  ];

  const validateForm = () => {
    const newErrors = {};
    
    if (!formData.descricao.trim()) {
      newErrors.descricao = 'Descrição é obrigatória';
    } else if (formData.descricao.length < 3) {
      newErrors.descricao = 'Descrição deve ter pelo menos 3 caracteres';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    setIsSubmitting(true);
    
    try {
      await categoriaService.create(formData);
      onSuccess();
    } catch (error) {
      console.error('Erro ao criar categoria:', error);
      alert('Erro ao criar categoria. Tente novamente.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="categoria-form">
      <h3>Cadastrar Nova Categoria</h3>
      
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
            placeholder="Digite a descrição da categoria"
          />
          {errors.descricao && <span className="error-text">{errors.descricao}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="finalidade">Finalidade *</label>
          <select
            id="finalidade"
            name="finalidade"
            value={formData.finalidade}
            onChange={handleChange}
          >
            {finalidades.map(finalidade => (
              <option key={finalidade.value} value={finalidade.value}>
                {finalidade.label}
              </option>
            ))}
          </select>
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

export default CategoriaForm;