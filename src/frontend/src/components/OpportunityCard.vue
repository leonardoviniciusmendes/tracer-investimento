<script setup>
defineProps({
  opportunity: {
    type: Object,
    required: true,
  },
});
</script>

<template>
  <article class="opportunity-card">
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
</template>
