<script setup>
import { resolveCurrentRoute } from "./router";

const currentRoute = resolveCurrentRoute();
</script>

<template>
  <component :is="currentRoute.component" />
</template>
