const apiBaseUrl = import.meta.env.VITE_API_URL ?? "http://localhost:8081";

export const fallbackOpportunities = [
  {
    ticker: "PETR4",
    companyName: "Petrobras PN",
    sector: "Energia",
    currentPrice: 31.42,
    targetPrice: 37.8,
    score: 84,
    upsidePercent: 20.31,
    thesis: "Fluxo de caixa robusto com payout relevante e valuation descontado.",
  },
  {
    ticker: "ITUB4",
    companyName: "Itau Unibanco PN",
    sector: "Financeiro",
    currentPrice: 34.15,
    targetPrice: 39.6,
    score: 81,
    upsidePercent: 15.96,
    thesis: "Qualidade operacional alta e recorrencia de resultado.",
  },
  {
    ticker: "WEGE3",
    companyName: "WEG ON",
    sector: "Industria",
    currentPrice: 48.9,
    targetPrice: 55,
    score: 78,
    upsidePercent: 12.47,
    thesis: "Crescimento resiliente conectado a tendencias estruturais.",
  },
];

export async function fetchTopOpportunities() {
  const response = await fetch(`${apiBaseUrl}/api/opportunities?minScore=70`);

  if (!response.ok) {
    throw new Error("Falha ao carregar oportunidades.");
  }

  const data = await response.json();

  return Array.isArray(data) ? data : [];
}
