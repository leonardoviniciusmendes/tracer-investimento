<script setup>
defineProps({
  opportunityCount: {
    type: Number,
    required: true,
  },
  averageScore: {
    type: Number,
    required: true,
  },
  dataSource: {
    type: String,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true,
  },
});
</script>

<template>
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
        <strong>{{ opportunityCount }}</strong>
        <span>oportunidades destacadas</span>
      </div>
      <div class="metric-card">
        <strong>{{ averageScore }}</strong>
        <span>score medio</span>
      </div>
      <div class="metric-card">
        <strong>{{ isLoading ? "atualizando" : dataSource }}</strong>
        <span>fonte atual</span>
      </div>
    </div>
  </section>
</template>
