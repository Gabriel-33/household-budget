//classe com objetivo de válidar campos de formulários com regex
export const validators = {
  required: (value) => !value ? 'Campo obrigatório' : null,
  
  //válida se o valor utilizado num formulário é um email válido
  email: (value) => {
    if (!value) return null;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return !emailRegex.test(value) ? 'E-mail inválido' : null;
  },
  
  //válida se a string num formulário possui a quantidade mínima de caracteres
  minLength: (min) => (value) => {
    if (!value) return null;
    return value.length < min ? `Mínimo ${min} caracteres` : null;
  },
  
  //válida se a string num formulário possui a quantidade máxima de caracteres
  maxLength: (max) => (value) => {
    if (!value) return null;
    return value.length > max ? `Máximo ${max} caracteres` : null;
  },
  
  //válida se um campo númerico, tem possui o valor minínimo
  minValue: (min) => (value) => {
    if (!value) return null;
    const num = parseFloat(value);
    return num < min ? `Valor mínimo: ${min}` : null;
  },
  
  //válida se um campo númerico, tem possui o valor máximo
  maxValue: (max) => (value) => {
    if (!value) return null;
    const num = parseFloat(value);
    return num > max ? `Valor máximo: ${max}` : null;
  },
  
  //válida se a idade de um user do sistema, é válida, para determinada transação.
  idade: (value) => {
    if (!value) return null;
    const idade = parseInt(value);
    if (isNaN(idade)) return 'Idade inválida';
    if (idade < 0) return 'Idade não pode ser negativa';
    if (idade > 150) return 'Idade máxima: 150 anos';
    return null;
  }
};