import React, { useState, useEffect } from 'react';
import {
  Container,
  Grid,
  Paper,
  Typography,
  Box,
  Card,
  CardContent,
  Button,
  CircularProgress
} from '@mui/material';
import {
  People,
  Category,
  Receipt,
  TrendingUp,
  TrendingDown,
  Add
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { pessoaService } from '../../services/pessoaService';
import { transacaoService } from '../../services/transacaoService';
import { relatorioService } from '../../services/relatorioService';
import { formatCurrency } from '../../utils/formatters';
import { toast } from 'react-hot-toast';

const Dashboard = () => {
  const [stats, setStats] = useState({
    totalPessoas: 0,
    totalTransacoes: 0,
    totalReceitas: 0,
    totalDespesas: 0
  });
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      const [pessoasRes, transacoesRes, relatorioRes] = await Promise.all([
        pessoaService.getPessoas(1, 1),
        transacaoService.getTransacoes(1, 1),
        relatorioService.getTotaisPorPessoa()
      ]);

      setStats({
        totalPessoas: pessoasRes.totalCount || 0,
        totalTransacoes: transacoesRes.totalCount || 0,
        totalReceitas: relatorioRes?.totalGeral?.totalReceitas || 0,
        totalDespesas: relatorioRes?.totalGeral?.totalDespesas || 0
      });
    } catch (error) {
      console.error('Erro ao carregar dashboard:', error);
      toast.error('Erro ao carregar dados do dashboard');
    } finally {
      setLoading(false);
    }
  };

  const quickActions = [
    { label: 'Nova Pessoa', icon: <People />, path: '/pessoas' },
    { label: 'Nova Categoria', icon: <Category />, path: '/categorias' },
    { label: 'Nova Transação', icon: <Receipt />, path: '/transacoes' },
    { label: 'Ver Relatórios', icon: <TrendingUp />, path: '/relatorios/pessoas' }
  ];

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Dashboard
      </Typography>

      <Grid container spacing={3} sx={{ mb: 4 }}>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center">
                <People sx={{ fontSize: 40, color: 'primary.main', mr: 2 }} />
                <Box>
                  <Typography color="textSecondary" variant="body2">
                    Pessoas
                  </Typography>
                  <Typography variant="h4">
                    {stats.totalPessoas}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center">
                <Receipt sx={{ fontSize: 40, color: 'secondary.main', mr: 2 }} />
                <Box>
                  <Typography color="textSecondary" variant="body2">
                    Transações
                  </Typography>
                  <Typography variant="h4">
                    {stats.totalTransacoes}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center">
                <TrendingUp sx={{ fontSize: 40, color: 'success.main', mr: 2 }} />
                <Box>
                  <Typography color="textSecondary" variant="body2">
                    Receitas
                  </Typography>
                  <Typography variant="h5" color="success.main">
                    {formatCurrency(stats.totalReceitas)}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box display="flex" alignItems="center">
                <TrendingDown sx={{ fontSize: 40, color: 'error.main', mr: 2 }} />
                <Box>
                  <Typography color="textSecondary" variant="body2">
                    Despesas
                  </Typography>
                  <Typography variant="h5" color="error.main">
                    {formatCurrency(stats.totalDespesas)}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      <Paper sx={{ p: 3, mb: 4 }}>
        <Typography variant="h6" gutterBottom>
          Ações Rápidas
        </Typography>
        <Grid container spacing={2}>
          {quickActions.map((action) => (
            <Grid item key={action.label}>
              <Button
                variant="outlined"
                startIcon={action.icon}
                onClick={() => navigate(action.path)}
                size="large"
              >
                {action.label}
              </Button>
            </Grid>
          ))}
        </Grid>
      </Paper>

      <Paper sx={{ p: 3 }}>
        <Typography variant="h6" gutterBottom>
          Funcionalidades do Sistema
        </Typography>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Box sx={{ p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
              <Typography variant="subtitle1" gutterBottom>
                <People sx={{ verticalAlign: 'middle', mr: 1 }} />
                Cadastro de Pessoas
              </Typography>
              <Typography variant="body2" color="textSecondary">
                • Criação, listagem e exclusão de pessoas
                <br />
                • Exclusão em cascata das transações
                <br />
                • Validação de idade (0-150 anos)
              </Typography>
            </Box>
          </Grid>

          <Grid item xs={12} md={6}>
            <Box sx={{ p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
              <Typography variant="subtitle1" gutterBottom>
                <Category sx={{ verticalAlign: 'middle', mr: 1 }} />
                Cadastro de Categorias
              </Typography>
              <Typography variant="body2" color="textSecondary">
                • Criação e listagem de categorias
                <br />
                • Finalidade: Despesa, Receita ou Ambas
                <br />
                • Compatibilidade com tipo de transação
              </Typography>
            </Box>
          </Grid>

          <Grid item xs={12} md={6}>
            <Box sx={{ p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
              <Typography variant="subtitle1" gutterBottom>
                <Receipt sx={{ verticalAlign: 'middle', mr: 1 }} />
                Cadastro de Transações
              </Typography>
              <Typography variant="body2" color="textSecondary">
                • Validação de menor de idade (apenas despesas)
                <br />
                • Compatibilidade categoria/tipo
                <br />
                • Valores positivos obrigatórios
              </Typography>
            </Box>
          </Grid>

          <Grid item xs={12} md={6}>
            <Box sx={{ p: 2, border: '1px solid #e0e0e0', borderRadius: 1 }}>
              <Typography variant="subtitle1" gutterBottom>
                <TrendingUp sx={{ verticalAlign: 'middle', mr: 1 }} />
                Relatórios
              </Typography>
              <Typography variant="body2" color="textSecondary">
                • Totais por pessoa (receitas, despesas, saldo)
                <br />
                • Totais por categoria
                <br />
                • Totais gerais consolidados
              </Typography>
            </Box>
          </Grid>
        </Grid>
      </Paper>
    </Container>
  );
};

export default Dashboard;