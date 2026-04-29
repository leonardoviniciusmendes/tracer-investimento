import { computed, reactive, ref } from "vue";
import {
  fallbackOpportunities,
  fetchTopOpportunities,
} from "../services/opportunitiesService";

export function useDashboardStore() {
  const opportunities = ref([...fallbackOpportunities]);
  const dataSource = ref("dados de demonstracao");
  const isLoading = ref(false);
  const errorMessage = ref("");
  const filters = reactive({
    sector: "",
    minScore: 70,
  });

  const sectors = computed(() => {
    const values = new Set(opportunities.value.map((item) => item.sector));
    return ["", ...values];
  });

  const averageScore = computed(() => {
    if (!opportunities.value.length) {
      return 0;
    }

    const total = opportunities.value.reduce((sum, item) => sum + item.score, 0);
    return Math.round(total / opportunities.value.length);
  });

  async function loadOpportunities() {
    isLoading.value = true;
    errorMessage.value = "";

    try {
      const data = await fetchTopOpportunities(filters);

      if (data.length > 0) {
        opportunities.value = data;
        dataSource.value = "API local";
        return;
      }

      opportunities.value = [];
      dataSource.value = "API local";
    } catch {
      opportunities.value = [...fallbackOpportunities];
      dataSource.value = "dados de demonstracao";
      errorMessage.value =
        "Nao foi possivel carregar a API local. Exibindo dados de demonstracao.";
    } finally {
      isLoading.value = false;
    }
  }

  return {
    opportunities,
    dataSource,
    isLoading,
    errorMessage,
    averageScore,
    filters,
    sectors,
    loadOpportunities,
  };
}
