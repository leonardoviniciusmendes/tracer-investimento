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
  errorMessage: {
    type: String,
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
      <div v-if="isLoading" class="status-banner loading-banner">
        Atualizando oportunidades...
      </div>
      <div v-else-if="errorMessage" class="status-banner error-banner">
        {{ errorMessage }}
      </div>
      <div v-else-if="!opportunities.length" class="status-banner empty-banner">
        Nenhuma oportunidade encontrada para os filtros atuais.
      </div>
      <template v-else>
        <OpportunityCard
          v-for="opportunity in opportunities"
          :key="opportunity.ticker"
          :opportunity="opportunity"
        />
      </template>
    </div>
  </article>
</template>
