import React, { useState } from 'react';
import { pessoaService } from '../../services/pessoaService';

const PessoaForm = ({ onSuccess, onCancel }) => {
  const [formData, setFormData] = useState({
    nome: '',
    idade: ''
  });
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  const validateForm = () => {
    const newErrors = {};
    
    if (!formData.nome.trim()) {
      newErrors.nome = 'Nome é obrigatório';
    }
    
    if (!formData.idade) {
      newErrors.idade = 'Idade é obrigatória';
    } else if (parseInt(formData.idade) < 0) {
      newErrors.idade = 'Idade deve ser um número positivo';
    } else if (parseInt(formData.idade) > 150) {
      newErrors.idade = 'Idade inválida';
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
      const pessoaData = {
        ...formData,
        idade: parseInt(formData.idade)
      };
      
      await pessoaService.create(pessoaData);
      onSuccess();
    } catch (error) {
      console.error('Erro ao criar pessoa:', error);
      alert('Erro ao criar pessoa. Tente novamente.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="pessoa-form">
      <h3>Cadastrar Nova Pessoa</h3>
      
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="nome">Nome *</label>
          <input
            type="text"
            id="nome"
            name="nome"
            value={formData.nome}
            onChange={handleChange}
            className={errors.nome ? 'error' : ''}
            placeholder="Digite o nome completo"
          />
          {errors.nome && <span className="error-text">{errors.nome}</span>}
        </div>
        
        <div className="form-group">
          <label htmlFor="idade">Idade *</label>
          <input
            type="number"
            id="idade"
            name="idade"
            value={formData.idade}
            onChange={handleChange}
            className={errors.idade ? 'error' : ''}
            placeholder="Digite a idade"
            min="0"
            max="150"
          />
          {errors.idade && <span className="error-text">{errors.idade}</span>}
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

export default PessoaForm;