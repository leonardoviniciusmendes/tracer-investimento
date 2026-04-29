<script setup>
import OpportunityCard from "./OpportunityCard.vue";

defineProps({
  opportunities: {
    type: Array,
    required: true,
  },
  isLoading: {
    type: Boolean,
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
      <div v-if="isLoading" class="empty-state">Atualizando oportunidades...</div>
      <div v-else-if="!opportunities.length" class="empty-state">
        Nenhuma oportunidade encontrada para os filtros atuais.
      </div>
      <OpportunityCard
        v-else
        v-for="opportunity in opportunities"
        :key="opportunity.ticker"
        :opportunity="opportunity"
      />
    </div>
  </article>
</template>
