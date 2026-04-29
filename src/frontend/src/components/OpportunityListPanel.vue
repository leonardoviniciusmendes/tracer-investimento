<script setup>
import OpportunityCard from "./OpportunityCard.vue";

defineProps({
  opportunities: {
    type: Array,
    required: true,
  },
});
</script>

<template>
  <article class="panel">
    <div class="panel-header">
      <div>
        <span class="eyebrow">Painel</span>
        <h2>Oportunidades priorizadas</h2>
      </div>
      <p>Leitura inicial de ativos com maior atratividade relativa.</p>
    </div>

    <div class="opportunity-list">
      <OpportunityCard
        v-for="opportunity in opportunities"
        :key="opportunity.ticker"
        :opportunity="opportunity"
      />
    </div>
  </article>
</template>
