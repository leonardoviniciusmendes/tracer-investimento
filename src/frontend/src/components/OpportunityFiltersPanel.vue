<script setup>
defineProps({
  filters: {
    type: Object,
    required: true,
  },
  sectors: {
    type: Array,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true,
  },
  summary: {
    type: String,
    required: true,
  },
});

defineEmits(["update:sector", "update:minScore", "apply", "reset"]);
</script>

<template>
  <article class="panel filters-panel">
    <div class="panel-header">
      <div>
        <span class="eyebrow">Filtros</span>
        <h2>Refine a leitura</h2>
      </div>
      <p>Ajuste setor e score minimo para mudar a lista em tempo real.</p>
    </div>

    <div class="filters-grid">
      <label class="field">
        <span>Setor</span>
        <select
          :value="filters.sector"
          :disabled="isLoading"
          @change="$emit('update:sector', $event.target.value)"
        >
          <option value="">Todos os setores</option>
          <option v-for="sector in sectors.filter(Boolean)" :key="sector" :value="sector">
            {{ sector }}
          </option>
        </select>
      </label>

      <label class="field">
        <span>Score minimo</span>
        <input
          :value="filters.minScore"
          :disabled="isLoading"
          type="number"
          min="0"
          max="100"
          step="1"
          @input="$emit('update:minScore', $event.target.value)"
        />
      </label>
    </div>

    <div class="filters-actions">
      <button class="ghost-button" type="button" :disabled="isLoading" @click="$emit('reset')">
        Limpar filtros
      </button>
      <button class="primary-button" type="button" :disabled="isLoading" @click="$emit('apply')">
        {{ isLoading ? "Atualizando..." : "Aplicar filtros" }}
      </button>
    </div>

    <div class="filters-summary" aria-live="polite">
      <span class="summary-chip">{{ summary }}</span>
    </div>
  </article>
</template>
