<script setup>
import { computed, onMounted, ref } from "vue";

const apiBaseUrl = import.meta.env.VITE_API_URL ?? "http://localhost:8080";

const fallbackOpportunities = [
  {
    ticker: "PETR4",
    companyName: "Petrobras PN",
    sector: "Energia",
    currentPrice: 31.42,
    targetPrice: 37.8,
    score: 84,
    upsidePercent: 20.31,
    thesis: "Fluxo de caixa robusto com payout relevante e valuation descontado.",
  },
  {
    ticker: "ITUB4",
    companyName: "Itau Unibanco PN",
    sector: "Financeiro",
    currentPrice: 34.15,
    targetPrice: 39.6,
    score: 81,
    upsidePercent: 15.96,
    thesis: "Qualidade operacional alta e recorrencia de resultado.",
  },
  {
    ticker: "WEGE3",
    companyName: "WEG ON",
    sector: "Industria",
    currentPrice: 48.9,
    targetPrice: 55,
    score: 78,
    upsidePercent: 12.47,
    thesis: "Crescimento resiliente conectado a tendencias estruturais.",
  },
];

const opportunities = ref(fallbackOpportunities);
const dataSource = ref("dados de demonstracao");

const averageScore = computed(() => {
  if (!opportunities.value.length) {
    return 0;
  }

  const total = opportunities.value.reduce((sum, item) => sum + item.score, 0);
  return Math.round(total / opportunities.value.length);
});

onMounted(async () => {
  try {
    const response = await fetch(`${apiBaseUrl}/api/opportunities?minScore=70`);

    if (!response.ok) {
      return;
    }

    const data = await response.json();

    if (Array.isArray(data) && data.length > 0) {
      opportunities.value = data;
      dataSource.value = "API local";
    }
  } catch {
    dataSource.value = "dados de demonstracao";
  }
});
</script>

<template>
  <main class="page-shell">
    <section class="hero">
      <div class="hero-copy">
        <span class="eyebrow">RadarBolsa | MVP inicial</span>
        <h1>Rastreie sinais e priorize oportunidades de investimento.</h1>
        <p>
          Base inicial para consolidar ativos monitorados, score de oportunidade
          e tese resumida em um painel unico.
        </p>
      </div>

      <div class="hero-panel">
        <div class="metric-card">
          <strong>{{ opportunities.length }}</strong>
          <span>oportunidades destacadas</span>
        </div>
        <div class="metric-card">
          <strong>{{ averageScore }}</strong>
          <span>score medio</span>
        </div>
        <div class="metric-card">
          <strong>{{ dataSource }}</strong>
          <span>fonte atual</span>
        </div>
      </div>
    </section>

    <section class="content-grid">
      <article class="panel">
        <div class="panel-header">
          <div>
            <span class="eyebrow">Painel</span>
            <h2>Oportunidades priorizadas</h2>
          </div>
          <p>Leitura inicial de ativos com maior atratividade relativa.</p>
        </div>

        <div class="opportunity-list">
          <article
            v-for="opportunity in opportunities"
            :key="opportunity.ticker"
            class="opportunity-card"
          >
            <div class="opportunity-topline">
              <div>
                <span class="ticker">{{ opportunity.ticker }}</span>
                <h3>{{ opportunity.companyName }}</h3>
              </div>
              <span class="score-badge">Score {{ opportunity.score }}</span>
            </div>

            <div class="opportunity-stats">
              <span>{{ opportunity.sector }}</span>
              <span>R$ {{ Number(opportunity.currentPrice).toFixed(2) }}</span>
              <span>Alvo R$ {{ Number(opportunity.targetPrice).toFixed(2) }}</span>
              <span class="upside">
                {{ Number(opportunity.upsidePercent).toFixed(2) }}% upside
              </span>
            </div>

            <p>{{ opportunity.thesis }}</p>
          </article>
        </div>
      </article>

      <aside class="panel">
        <div class="panel-header">
          <div>
            <span class="eyebrow">Roadmap</span>
            <h2>Proximas entregas</h2>
          </div>
          <p>Evolucao sugerida logo apos a estrutura inicial.</p>
        </div>

        <ol class="roadmap-list">
          <li>Persistir ativos e oportunidades em MySQL.</li>
          <li>Criar ingestion manual e agendada de sinais.</li>
          <li>Adicionar filtros por setor, score e upside.</li>
          <li>Incluir autenticacao e watchlists por usuario.</li>
        </ol>
      </aside>
    </section>
  </main>
</template>
