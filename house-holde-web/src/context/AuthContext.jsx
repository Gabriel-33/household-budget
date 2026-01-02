import React, { createContext, useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';

const AuthContext = createContext({});

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // Função para buscar informações completas do usuário
  const fetchUserDetails = async (userId) => {
    try {
      // Se você tiver um endpoint para buscar usuário por ID
      // const response = await api.get(`/user/${userId}`);
      // return response.data;
      
      // Por enquanto, retornamos o usuário básico do localStorage
      const user = authService.getCurrentUser();
      return user;
    } catch (error) {
      console.error('Erro ao buscar detalhes do usuário:', error);
      return null;
    }
  };

  useEffect(() => {
    const initializeAuth = async () => {
      const currentUser = authService.getCurrentUser();
      
      if (currentUser) {
        // Tenta buscar mais informações do usuário se necessário
        const userDetails = await fetchUserDetails(currentUser.id);
        setUser(userDetails || currentUser);
      }
      
      setLoading(false);
    };

    initializeAuth();
  }, []);

  const login = async (credentials) => {
    try {
      console.log('Tentando login com:', credentials);
      const data = await authService.login(credentials);
      
      if (data.token) {
        // Atualiza o estado do usuário com as informações que temos
        setUser(data.user);
        
        // Redireciona para a página inicial
        setTimeout(() => {
          navigate('/', { replace: true });
        }, 100);
        
        return { success: true, user: data.user };
      } else {
        return { success: false, error: 'Token não recebido' };
      }
    } catch (error) {
      console.error('Erro no login:', error);
      
      let errorMessage = 'Erro ao fazer login';
      
      if (error.response) {
        // O servidor respondeu com um status de erro
        const errorData = error.response.data;
        errorMessage = errorData?.message || 
                      errorData?.errors?.[0] || 
                      `Erro ${error.response.status}: ${error.response.statusText}`;
      } else if (error.request) {
        // A requisição foi feita mas não houve resposta
        errorMessage = 'Servidor não respondeu. Verifique sua conexão.';
      } else {
        // Algo aconteceu na configuração da requisição
        errorMessage = error.message;
      }
      
      return { success: false, error: errorMessage };
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
    navigate('/login', { replace: true });
  };

  const updateUser = (userData) => {
    setUser(prev => ({ ...prev, ...userData }));
    localStorage.setItem('user', JSON.stringify({ ...user, ...userData }));
  };

  return (
    <AuthContext.Provider value={{ 
      user, 
      login, 
      logout, 
      updateUser,
      isAuthenticated: !!user,
      loading 
    }}>
      {children}
    </AuthContext.Provider>
  );
};