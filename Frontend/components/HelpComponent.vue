<script setup>
	//componente para pedir ayuda
	import helpClassic from '~/assets/img/themes/classicTheme/help.png';
	
	import helpDark from '~/assets/img/themes/darkTheme/helpDark.png';

	const { currentTheme } = useCurrentTheme();

	const imgByTheme = {
		classicTheme: {
			help: helpClassic
		},
		darkTheme: {
			help: helpDark
		}	
  };

	const currentThemeComputed = computed(() => {
    return currentTheme.value;
  });

	const disabled = ref(false);

	const emit = defineEmits(['help']);
	const submitHelp = () => {
		disabled.value = true;
		setTimeout(() => {
			disabled.value = false;
		}, 2000); 
		emit('help');
	};
</script>

<template>
	<div class="button-border">
		<button :disabled="disabled" @click="submitHelp" 
						class="button w-[38px] h-[38px]"
						:title="$t('header.help')"
		>
			<img :src="imgByTheme[currentThemeComputed].help" alt="Help" />
		</button>
	</div>
</template>