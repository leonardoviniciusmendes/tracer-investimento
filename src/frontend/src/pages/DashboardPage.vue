<script setup>
import { onMounted, watch } from "vue";
import HeroOverview from "../components/HeroOverview.vue";
import OpportunityFiltersPanel from "../components/OpportunityFiltersPanel.vue";
import OpportunityListPanel from "../components/OpportunityListPanel.vue";
import RoadmapPanel from "../components/RoadmapPanel.vue";
import { useDashboardStore } from "../stores/useDashboardStore";

const {
  opportunities,
  dataSource,
  isLoading,
  averageScore,
  loadOpportunities,
} = useDashboardStore();

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
          @update:sector="filters.sector = $event"
          @update:minScore="filters.minScore = $event"
          @apply="loadOpportunities"
          @reset="resetFilters"
        />
        <OpportunityListPanel
          :opportunities="opportunities"
          :is-loading="isLoading"
        />
      </div>
      <RoadmapPanel />
    </section>
  </main>
</template>
