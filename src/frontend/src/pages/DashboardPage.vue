<script setup>
import { computed, onMounted, watch } from "vue";
import HeroOverview from "../components/HeroOverview.vue";
import OpportunityFiltersPanel from "../components/OpportunityFiltersPanel.vue";
import OpportunityListPanel from "../components/OpportunityListPanel.vue";
import RoadmapPanel from "../components/RoadmapPanel.vue";
import { useDashboardStore } from "../stores/useDashboardStore";

const {
  opportunities,
  dataSource,
  isLoading,
  errorMessage,
  averageScore,
  filters,
  sectors,
  loadOpportunities,
} = useDashboardStore();

const activeFilterSummary = computed(() => {
  const sectorLabel = filters.sector ? filters.sector : "Todos os setores";
  return `${sectorLabel} | Score >= ${filters.minScore}`;
});

onMounted(loadOpportunities);

watch(
  () => [filters.sector, filters.minScore],
  () => {
    loadOpportunities();
  },
);

function resetFilters() {
  filters.sector = "";
  filters.minScore = 70;
}
</script>

<template>
  <main class="page-shell">
    <HeroOverview
      :opportunity-count="opportunities.length"
      :average-score="averageScore"
      :data-source="dataSource"
      :is-loading="isLoading"
    />

    <section class="content-grid">
      <div class="stack">
        <OpportunityFiltersPanel
          :filters="filters"
          :sectors="sectors"
          :is-loading="isLoading"
          :summary="activeFilterSummary"
          @update:sector="filters.sector = $event"
          @update:minScore="filters.minScore = $event"
          @apply="loadOpportunities"
          @reset="resetFilters"
        />
        <OpportunityListPanel
          :opportunities="opportunities"
          :is-loading="isLoading"
          :error-message="errorMessage"
        />
      </div>
      <RoadmapPanel />
    </section>
  </main>
</template>
