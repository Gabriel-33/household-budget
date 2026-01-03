import api from './api';

export const authService = {
  async login(credentials) {
    console.log('Enviando login:', credentials);
    
    const loginData = {
      email: credentials.email,
      password: credentials.password
    };
    
    try {
      const response = await api.post('/auth/login', loginData);
      console.log('Resposta completa do login:', response.data);
      
      const { tokenJwt, idUsuario } = response.data;
      
      if (tokenJwt) {
        // Salva o token (note que vem como tokenJwt, não token)
        localStorage.setItem('token', tokenJwt);
        
        // Cria um objeto de usuário básico com as informações que temos
        const user = {
          id: idUsuario,
          email: credentials.email,
          // Como não recebemos nome do backend, podemos usar o email ou solicitar depois
          name: credentials.email.split('@')[0] // Extrai nome do email
        };
        
        localStorage.setItem('user', JSON.stringify(user));
        console.log('Token salvo:', tokenJwt);
        console.log('Usuário salvo:', user);
      } else {
        console.warn('Token não recebido na resposta');
        throw new Error('Token não recebido');
      }
      
      return {
        token: tokenJwt,
        user: {
          id: idUsuario,
          email: credentials.email,
          name: credentials.email.split('@')[0]
        }
      };
    } catch (error) {
      console.error('Erro no authService.login:', error);
      throw error;
    }
  },

  async register(credentials) {
    console.log('Enviando login:', credentials);
    
    const registerData = {
      username: credentials.name,
      email: credentials.email,
      password: credentials.password,
      role:2
    };
    
    try {
      const response = await api.post('/auth/register', registerData);
      console.log('Resposta completa do login:', response.data);
      
      const { tokenJwt, idUsuario } = response.data;
      
      if (tokenJwt) {
        // Salva o token (note que vem como tokenJwt, não token)
        localStorage.setItem('token', tokenJwt);
        
        // Cria um objeto de usuário básico com as informações que temos
        const user = {
          id: idUsuario,
          email: credentials.email,
          // Como não recebemos nome do backend, podemos usar o email ou solicitar depois
          name: credentials.email.split('@')[0] // Extrai nome do email
        };
        
        localStorage.setItem('user', JSON.stringify(user));
        console.log('Token salvo:', tokenJwt);
        console.log('Usuário salvo:', user);
      } else {
        console.warn('Token não recebido na resposta');
        throw new Error('Token não recebido');
      }
      
      return {
        token: tokenJwt,
        user: {
          id: idUsuario,
          email: credentials.email,
          name: credentials.email.split('@')[0]
        }
      };
    } catch (error) {
      console.error('Erro no authService.login:', error);
      throw error;
    }
  },

  logout() {
    console.log('Logout executado');
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getCurrentUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },

  isAuthenticated() {
    return !!localStorage.getItem('token');
  }
};