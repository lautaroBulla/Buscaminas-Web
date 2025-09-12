<script setup>
	//para ver las configraciones del juego
	import settingClassic from '~/assets/img/themes/classicTheme/settings.png';

	import settingDark from '~/assets/img/themes/darkTheme/settingsDark.png';
	
	const { currentTheme } = useCurrentTheme();

	const imgByTheme = {
		classicTheme: {
			setting: settingClassic
		},
		darkTheme: {
			setting: settingDark
		}
  };

	const currentThemeComputed = computed(() => {
    return currentTheme.value;
  });
</script>

<template>
	<div class="button-border">
		<button @click="$emit('view-settings')" 
						class="button w-[38px] h-[38px]"
						:title="$t('header.settings')"
		>
			<img :src="imgByTheme[currentThemeComputed].setting" alt="Settings"/>
		</button>
	</div>
</template>