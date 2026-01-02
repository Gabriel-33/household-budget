export const validators = {
  required: (value) => !value ? 'Campo obrigatório' : null,
  
  email: (value) => {
    if (!value) return null;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return !emailRegex.test(value) ? 'E-mail inválido' : null;
  },
  
  minLength: (min) => (value) => {
    if (!value) return null;
    return value.length < min ? `Mínimo ${min} caracteres` : null;
  },
  
  maxLength: (max) => (value) => {
    if (!value) return null;
    return value.length > max ? `Máximo ${max} caracteres` : null;
  },
  
  minValue: (min) => (value) => {
    if (!value) return null;
    const num = parseFloat(value);
    return num < min ? `Valor mínimo: ${min}` : null;
  },
  
  maxValue: (max) => (value) => {
    if (!value) return null;
    const num = parseFloat(value);
    return num > max ? `Valor máximo: ${max}` : null;
  },
  
  idade: (value) => {
    if (!value) return null;
    const idade = parseInt(value);
    if (isNaN(idade)) return 'Idade inválida';
    if (idade < 0) return 'Idade não pode ser negativa';
    if (idade > 150) return 'Idade máxima: 150 anos';
    return null;
  }
};