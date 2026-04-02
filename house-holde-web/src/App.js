import React, { Suspense } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import PrivateRoute from './components/Auth/PrivateRoute';
import Login from './components/Auth/Login';
import Layout from './components/Layout/Layout';
import PessoasList from './components/Pessoas/PessoasList';
import CategoriasList from './components/Categorias/CategoriasList';
import TransacoesList from './components/Transacoes/TransacoesList';
import RelatorioPessoas from './components/Relatorios/RelatorioPessoas';
import RelatorioCategorias from './components/Relatorios/RelatorioCategorias';
import './App.css';

// Componente de loading para Suspense
const LoadingFallback = () => (
  <div className="loading-screen">
    <div className="spinner"></div>
    <p>Carregando...</p>
  </div>
);

function App() {
  return (
    <Router>
      <AuthProvider>
        <Suspense fallback={<LoadingFallback />}>
          <Routes>
            <Route path="/login" element={<Login />} />
            
            {/* Rotas protegidas */}
            <Route path="/" element={
              <PrivateRoute>
                <Layout />
              </PrivateRoute>
            }>
              <Route index element={<Navigate to="/transacoes" replace />} />
              <Route path="pessoas" element={<PessoasList />} />
              <Route path="categorias" element={<CategoriasList />} />
              <Route path="transacoes" element={<TransacoesList />} />
              <Route path="relatorios/pessoas" element={<RelatorioPessoas />} />
              <Route path="relatorios/categorias" element={<RelatorioCategorias />} />
            </Route>
            
            {/* Rota de fallback para 404 */}
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Suspense>
      </AuthProvider>
    </Router>
  );
}

export default App;