import { Navigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
//componente para válidar autenticidade do usuário
const PrivateRoute = ({ children }) => {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return <div>Carregando...</div>;
  }

  return isAuthenticated ? children : <Navigate to="/login" />;
};

export default PrivateRoute;