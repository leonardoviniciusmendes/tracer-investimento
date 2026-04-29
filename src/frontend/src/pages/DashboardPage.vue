<script setup>
import { onMounted } from "vue";
import HeroOverview from "../components/HeroOverview.vue";
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
      <OpportunityListPanel :opportunities="opportunities" />
      <RoadmapPanel />
    </section>
  </main>
</template>
