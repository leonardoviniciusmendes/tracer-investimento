import DashboardPage from "../pages/DashboardPage.vue";

export const routes = Object.freeze([
  {
    name: "dashboard",
    path: "/",
    component: DashboardPage,
  },
]);

export function resolveCurrentRoute() {
  return routes[0];
}
